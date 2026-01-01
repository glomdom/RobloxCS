using System.Collections;

namespace RobloxCS.Types;

// screw this dumb stupid fucking api
public sealed class LuaTuple : IEnumerable<object> {
    public object[] Values { get; }

    public LuaTuple(params object[] values) {
        Values = values;
    }

    public T Get<T>(int index) => (T)Values[index];

    public void Deconstruct(out object a, out object b) {
        a = Values.Length > 0 ? Values[0] : null!;
        b = Values.Length > 1 ? Values[1] : null!;
    }

    public void Deconstruct(out object a, out object b, out object c) {
        a = Values.Length > 0 ? Values[0] : null!;
        b = Values.Length > 1 ? Values[1] : null!;
        c = Values.Length > 2 ? Values[2] : null!;
    }
    
    public void Deconstruct(out object a, out object b, out object c, out object d) {
        a = Values.Length > 0 ? Values[0] : null!;
        b = Values.Length > 1 ? Values[1] : null!;
        c = Values.Length > 2 ? Values[2] : null!;
        d = Values.Length > 3 ? Values[3] : null!;
    }

    public IEnumerator<object> GetEnumerator() => ((IEnumerable<object>)Values).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
}