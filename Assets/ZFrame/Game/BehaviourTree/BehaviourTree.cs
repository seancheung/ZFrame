using System;
using System.Linq;

namespace ZFrame
{
    public class BehaviourTree
    {
        /// <summary>
        /// Invoke until one func returns false
        /// </summary>
        /// <param name="funcs"></param>
        /// <returns></returns>
        public Func<bool> Sequence(params Func<bool>[] funcs)
        {
            return () => funcs.All(func => func.Invoke());
        }

        /// <summary>
        /// Invoke until one func returns true
        /// </summary>
        /// <param name="funcs"></param>
        /// <returns></returns>
        public Func<bool> Selector(params Func<bool>[] funcs)
        {
            return () => funcs.Any(func => func.Invoke());
        }

        /// <summary>
        /// Invoke by priority
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        public Func<bool> PrioritySelector(Func<int> priority, params Action[] actions)
        {
            return () =>
            {
                int index = priority.Invoke();
                if (actions.Length <= index || index < 0)
                    return false;

                actions[index].Invoke();
                return true;
            };
        }

        /// <summary>
        /// Invoke if condiction returns true
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Func<bool> Decorator(Func<bool> condition, Action action)
        {
            return () =>
            {
                if (!condition.Invoke())
                    return false;

                action.Invoke();
                return true;
            };
        }

        /// <summary>
        /// Invoke all
        /// </summary>
        /// <param name="actions"></param>
        public Action Parallel(params Action[] actions)
        {
            return () =>
            {
                foreach (Action action in actions)
                {
                    action.Invoke();
                }
            };
        }

        /// <summary>
        /// Warp up a func to action
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Action Wrap(Func<bool> func)
        {
            return () => func.Invoke();
        }

        /// <summary>
        /// Invert result
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Func<bool> Invert(Func<bool> func)
        {
            return () => !func.Invoke();
        }
    }
}