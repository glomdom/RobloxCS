using System;
using RobloxCS.Types.Attributes;

namespace InterfaceOne {
    internal interface IView {
        int Read();
    }
}

namespace InterfaceTwo {
    internal interface IView {
        int Read();
    }
}

namespace RobloxCS.Tests.Data {
    internal class NamedViews : InterfaceOne.IView, InterfaceTwo.IView {
        int InterfaceOne.IView.Read() {
            return 1;
        }

        int InterfaceTwo.IView.Read() {
            return 2;
        }
    }

    internal interface IVirtualState {
        int Value { get; set; }
    }

    internal class VirtualStateBase : IVirtualState {
        public virtual int Value { get; set; } = 5;
    }

    internal class VirtualStateDerived : VirtualStateBase {
        public override int Value {
            get => base.Value + 10;
            set => base.Value = value - 10;
        }
    }

    internal class InterfaceNames {
        [EntryPoint]
        internal static void Run() {
            var item = new NamedViews();
            InterfaceOne.IView first = item;
            InterfaceTwo.IView second = item;

            Console.WriteLine(first.Read());
            Console.WriteLine(second.Read());
            Console.WriteLine((InterfaceTwo.IView)first == second);

            IVirtualState state = new VirtualStateDerived();

            Console.WriteLine(state.Value);

            state.Value = 30;

            Console.WriteLine(state.Value);
        }
    }
}