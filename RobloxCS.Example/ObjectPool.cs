using RobloxCS.Types;

namespace RobloxCS.Example;

public class ObjectPool {
    public List<BasePart> Pool = [];

    public BasePart Get() {
        BasePart inst;

        if (Pool.Count > 0) {
            var last = Pool.Count - 1;
            inst = Pool[last];

            Pool.RemoveAt(last);
        } else {
            inst = new BasePart();
        }

        return inst;
    }

    public void Release(BasePart inst) {
        inst.Parent = null!;

        Pool.Add(inst);
    }
}