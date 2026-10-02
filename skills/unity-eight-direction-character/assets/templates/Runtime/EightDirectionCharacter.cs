using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D.Animation;

namespace RpgEightDirection
{
    [Serializable]
    public sealed class DirectionalResolverBinding
    {
        public string category;
        public SpriteResolver resolver;
        public bool equipmentControlled;
        public EquipmentSlot slot;
    }

    // Call SetScreenVelocity during Update before this component's Update.
    [DefaultExecutionOrder(100)]
    public sealed partial class EightDirectionCharacter : MonoBehaviour
    {
        public Animator animator;
        public SpriteLibrary library;
        public DirectionalCharacterProfile profile;
        public DirectionalResolverBinding[] bindings = new DirectionalResolverBinding[0];
        public Transform weaponR, weaponL;
        public SortingGroup legL, legR, armL, armR;
        public Direction8 initialDirection = Direction8.South;
        [Min(0f)] public float deadZone = 0.01f;
        [Range(0f, 15f)] public float hysteresisDegrees = 5f;
        public bool useInspectorTest;
        public Vector2 testScreenVelocity;
        public DirectionalEquipmentDefinition testItem;
        [HideInInspector] public float gaitPhase;

        private readonly Dictionary<EquipmentSlot, DirectionalEquipmentDefinition> equipped =
            new Dictionary<EquipmentSlot, DirectionalEquipmentDefinition>();
        private Vector2 requestedVelocity;
        private DirectionPose activePose;
        private bool isWalking;
        private string lastError;

        public Direction8 Facing { get { return activePose != null ? activePose.direction : initialDirection; } }

        private sealed class AppearancePlan
        {
            public DirectionPose pose;
            public readonly Dictionary<string, string> labels = new Dictionary<string, string>();
            public readonly Dictionary<string, int> orders = new Dictionary<string, int>();
            public SocketPose rightSocket, leftSocket;
        }

        private void Start()
        {
            if (animator == null) animator = GetComponent<Animator>();
            string error;
            if (!TryChangeState(initialDirection, false, out error))
            {
                Debug.LogError(error, this);
                enabled = false;
            }
        }

        public void SetScreenVelocity(Vector2 cameraProjectedVelocity)
        {
            requestedVelocity = Direction8Utility.IsFinite(cameraProjectedVelocity)
                ? cameraProjectedVelocity : Vector2.zero;
        }

        private void Update()
        {
            if (activeAction != null) { UpdateAction(); return; }
            Vector2 velocity = useInspectorTest ? testScreenVelocity : requestedVelocity;
            if (!Direction8Utility.IsFinite(velocity)) velocity = Vector2.zero;
            bool walking = velocity.magnitude > Mathf.Max(0f, deadZone);
            Direction8 direction = Direction8Utility.Select(velocity, Facing, deadZone, hysteresisDegrees);
            if (activePose != null && direction == Facing && walking == isWalking) return;
            string error;
            if (!TryChangeState(direction, walking, out error))
            {
                if (error != lastError) Debug.LogError(error, this);
                lastError = error;
            }
            else lastError = null;
        }

        private bool TryChangeState(Direction8 direction, bool walking, out string error)
        {
            int state = Animator.StringToHash("Base Layer." + (walking ? "Walk" : "Idle") + direction);
            if (animator == null || !animator.HasState(0, state))
            {
                error = "Missing Animator layer-0 state: " + (walking ? "Walk" : "Idle") + direction;
                return false;
            }
            AppearancePlan plan;
            if (!TryPrepare(direction, equipped, out plan, out error)) return false;
            float phase = isWalking && walking
                ? Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f) : 0f;
            Commit(plan);
            activePose = plan.pose;
            isWalking = walking;
            animator.Play(state, 0, phase);
            return true;
        }

        public bool TryEquip(DirectionalEquipmentDefinition item, out string error)
        {
            if (activeAction != null && !activeAction.allowEquipmentSwap)
            { error = "This action locks equipment until released."; return false; }
            if (activePose == null || item == null)
            {
                error = "Initialize the character and supply an item before equipping.";
                return false;
            }
            var proposed = new Dictionary<EquipmentSlot, DirectionalEquipmentDefinition>(equipped);
            proposed[item.slot] = item;
            AppearancePlan plan;
            if (!TryPrepare(Facing, proposed, out plan, out error)) return false;
            Commit(plan); // No Animator.Play, no bones changed: gait time continues.
            equipped[item.slot] = item;
            return true;
        }

        public bool TryUnequip(EquipmentSlot slot, out string error)
        {
            if (activeAction != null && !activeAction.allowEquipmentSwap)
            { error = "This action locks equipment until released."; return false; }
            if (activePose == null) { error = "Character is not initialized."; return false; }
            var proposed = new Dictionary<EquipmentSlot, DirectionalEquipmentDefinition>(equipped);
            proposed.Remove(slot);
            AppearancePlan plan;
            if (!TryPrepare(Facing, proposed, out plan, out error)) return false;
            Commit(plan);
            equipped.Remove(slot);
            return true;
        }

        // Useful in edit-mode setup and for validating every view of an item.
        public bool TryValidateDirection(Direction8 direction, out string error,
            DirectionalEquipmentDefinition item = null)
        {
            var items = new Dictionary<EquipmentSlot, DirectionalEquipmentDefinition>(equipped);
            if (item != null) items[item.slot] = item;
            AppearancePlan plan;
            return TryPrepare(direction, items, out plan, out error);
        }

        private bool TryPrepare(Direction8 direction,
            Dictionary<EquipmentSlot, DirectionalEquipmentDefinition> items,
            out AppearancePlan plan, out string error)
        {
            plan = null; error = null;
            DirectionPose pose;
            if (profile == null || !profile.TryGet(direction, out pose) || library == null ||
                library.transform != transform.Find("Visual") || library.GetComponent<SortingGroup>() == null ||
                bindings == null || bindings.Length == 0 || pose.defaults == null || pose.rendererOrders == null ||
                pose.walkDepth == null || pose.walkDepth.Length != 4)
                return Fail("Configure eight unique profiles, complete defaults/orders, and four walk depths.", out error);
            if (legL == null || legR == null || armL == null || armR == null ||
                legL.transform != transform.Find(CharacterRigPaths.ThighL) ||
                legR.transform != transform.Find(CharacterRigPaths.ThighR) ||
                armL.transform != transform.Find(CharacterRigPaths.ArmL) ||
                armR.transform != transform.Find(CharacterRigPaths.ArmR))
                return Fail("Assign limb SortingGroups to their matching physical bones.", out error);
            if (weaponR == null || weaponL == null ||
                transform.Find(CharacterRigPaths.HandR) == null ||
                transform.Find(CharacterRigPaths.HandL) == null ||
                weaponR.parent != transform.Find(CharacterRigPaths.HandR) ||
                weaponL.parent != transform.Find(CharacterRigPaths.HandL) ||
                weaponR.GetComponentInParent<SpriteLibrary>() != library ||
                weaponL.GetComponentInParent<SpriteLibrary>() != library)
                return Fail("Weapon sockets must be beneath their physical hands and the shared library.", out error);

            var result = new AppearancePlan { pose = pose, rightSocket = pose.weaponR, leftSocket = pose.weaponL };
            var registered = new Dictionary<string, DirectionalResolverBinding>();
            var resolvers = new HashSet<SpriteResolver>();
            foreach (var binding in bindings)
            {
                if (binding == null || string.IsNullOrWhiteSpace(binding.category) ||
                    registered.ContainsKey(binding.category) || binding.resolver == null ||
                    !resolvers.Add(binding.resolver) ||
                    binding.resolver.GetCategory() != binding.category ||
                    binding.resolver.GetComponent<SpriteRenderer>() == null ||
                    binding.resolver.GetComponentInParent<SpriteLibrary>() != library)
                    return Fail("Bindings need unique categories/resolvers and the shared library.", out error);
                if (binding.equipmentControlled &&
                    (binding.slot == EquipmentSlot.Weapon_R || binding.slot == EquipmentSlot.Weapon_L))
                {
                    Transform socket = binding.slot == EquipmentSlot.Weapon_R ? weaponR : weaponL;
                    if (binding.resolver.transform != socket && !binding.resolver.transform.IsChildOf(socket))
                        return Fail("Weapon resolver is outside its hand socket: " + binding.category, out error);
                }
                registered.Add(binding.category, binding);
            }
            foreach (string category in CharacterRigPaths.SpriteCategories)
                if (!registered.ContainsKey(category))
                    return Fail("Missing required resolver category: " + category, out error);
            foreach (var choice in pose.defaults)
            {
                if (string.IsNullOrWhiteSpace(choice.category) || string.IsNullOrWhiteSpace(choice.label) ||
                    !registered.ContainsKey(choice.category) || result.labels.ContainsKey(choice.category))
                    return Fail("Invalid/duplicate default category in " + direction, out error);
                result.labels.Add(choice.category, choice.label);
            }
            foreach (var order in pose.rendererOrders)
            {
                if (string.IsNullOrWhiteSpace(order.category) || !registered.ContainsKey(order.category) ||
                    result.orders.ContainsKey(order.category))
                    return Fail("Invalid/duplicate renderer order in " + direction, out error);
                result.orders.Add(order.category, order.order);
            }
            if (result.labels.Count != registered.Count || result.orders.Count != registered.Count)
                return Fail("Defaults and renderer orders must cover every binding in " + direction, out error);

            foreach (var pair in items)
            {
                DirectionItemAppearance appearance;
                if (pair.Value == null || !pair.Value.TryGet(direction, out appearance) || appearance.sprites == null)
                    return Fail("Item needs eight unique variants: " + pair.Key + " / " + direction, out error);
                var categories = new HashSet<string>();
                int expected = 0;
                foreach (var binding in bindings)
                    if (binding.equipmentControlled && binding.slot == pair.Key) expected++;
                if (expected == 0 || appearance.sprites.Length != expected)
                    return Fail("Incomplete item categories: " + pair.Value.itemId + " / " + direction, out error);
                foreach (var choice in appearance.sprites)
                {
                    DirectionalResolverBinding binding;
                    if (string.IsNullOrWhiteSpace(choice.category) || string.IsNullOrWhiteSpace(choice.label) ||
                        !categories.Add(choice.category) || !registered.TryGetValue(choice.category, out binding) ||
                        !binding.equipmentControlled || binding.slot != pair.Key)
                        return Fail("Invalid item category: " + pair.Value.itemId + " / " + direction, out error);
                    result.labels[choice.category] = choice.label;
                }
                if (pair.Key == EquipmentSlot.Weapon_R) result.rightSocket = appearance.weaponSocket;
                if (pair.Key == EquipmentSlot.Weapon_L) result.leftSocket = appearance.weaponSocket;
            }
            foreach (var pair in result.labels)
                if (library.GetSprite(pair.Key, pair.Value) == null)
                    return Fail("Missing sprite: " + direction + " / " + pair.Key + " / " + pair.Value, out error);
            if (!result.rightSocket.IsValid() || !result.leftSocket.IsValid())
                return Fail("Weapon socket poses need finite values and positive scale in " + direction, out error);
            plan = result;
            return true;
        }

        private static bool Fail(string message, out string error) { error = message; return false; }

        private void Commit(AppearancePlan plan)
        {
            // No visual mutations occur until all data has passed prevalidation.
            foreach (var binding in bindings)
            {
                binding.resolver.SetCategoryAndLabel(binding.category, plan.labels[binding.category]);
                binding.resolver.ResolveSpriteToSpriteRenderer();
                binding.resolver.GetComponent<SpriteRenderer>().sortingOrder = plan.orders[binding.category];
            }
            plan.rightSocket.Apply(weaponR);
            plan.leftSocket.Apply(weaponL);
        }

        private void LateUpdate()
        {
            if (activePose == null) return;
            if (TryApplyActionDepth()) return;
            int index = Mathf.Min(3, Mathf.FloorToInt(Mathf.Repeat(gaitPhase, 1f) * 4f));
            LimbOrders orders = isWalking ? activePose.walkDepth[index] : activePose.idleDepth;
            legL.sortingOrder = orders.legL; legR.sortingOrder = orders.legR;
            armL.sortingOrder = orders.armL; armR.sortingOrder = orders.armR;
        }

        [ContextMenu("Equip Test Item")]
        private void EquipTestItem()
        {
            if (!Application.isPlaying) return;
            string error;
            if (!TryEquip(testItem, out error)) Debug.LogError(error, this);
        }
    }
}
