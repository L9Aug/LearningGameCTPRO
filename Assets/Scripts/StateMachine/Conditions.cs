using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Condition
{
    public interface ICondition
    {
        bool Test();
    }

    public class BoolCondition : ICondition
    {
        public delegate bool BoolParameter();
        public BoolParameter Condition;

        BoolCondition() { }

        BoolCondition(BoolParameter condition)
        {
            Condition = condition;
        }

        public bool Test()
        {
            return Condition();
        }
    }

    public class AndCondition : ICondition
    {
        public ICondition A;
        public ICondition B;

        public AndCondition() { }

        public AndCondition(ICondition a, ICondition b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A.Test() && B.Test();
        }
    }

    public class OrCondition : ICondition
    {
        public ICondition A;
        public ICondition B;

        public OrCondition() { }

        public OrCondition(ICondition a, ICondition b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A.Test() || B.Test();
        }
    }

    public class NotCondition : ICondition
    {
        public ICondition Condition;

        public NotCondition() { }

        public NotCondition(ICondition condition)
        {
            Condition = condition;
        }

        public bool Test()
        {
            return !Condition.Test();
        }
    }


}
