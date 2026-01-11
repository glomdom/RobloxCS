using RobloxCS.Types;

namespace RobloxCS.Example;

public class ObjectPool {
    public List<Part> Pool = [];

    public Part Get() {
        Part inst;

        if (Pool.Count > 0) {
            var last = Pool.Count - 1;
            inst = Pool[last];

            Pool.RemoveAt(last);
        } else {
            inst = new Part();
        }

        return inst;
    }

    public void Release(Part inst) {
        inst.Parent = null!;

        Pool.Add(inst);
    }
}