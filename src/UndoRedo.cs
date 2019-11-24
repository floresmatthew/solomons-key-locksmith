using Romulus;
using Locksmith.Data;

namespace Locksmith
{
    /// <summary>
    /// Define our do/undo queue.
    /// </summary>
    class UndoRedo : ActionQueue<UndoRedo, UndoRedo.SolAction>
    {
        public SolRom Rom { get; private set; }

        public UndoRedo(SolRom rom) {
            this.Rom = rom;
        }

        /// <summary>
        /// This action class accepts a delegate to specify the do and undo functions.
        /// </summary>
        public class SolAction : UndoRedo.Action
        {
            string text;
            public SolAction(SimpleAction @do, SimpleAction undo, string text)
                : base(@do, undo) {
                this.text = text;
            }
            public override string Text {
                get { return text; }
            }
        }

        /// <summary>
        /// This is an example of how to create an action class that performs a specific action.
        /// </summary>
        public class SpecifiSolAction : UndoRedo.Action
        {
            protected override void PerformDo() {
                base.PerformDo();

                // This is where we put our "do" code
            }
            protected override void PerformUndo() {
                base.PerformUndo();

                // This is where we put our "undo" code
            }

            public override string Text {
                get { return "Description"; }
            }
        }
    }
}
