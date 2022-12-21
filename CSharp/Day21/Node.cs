using System.Diagnostics;

namespace Day21
{
    public enum Operation
    {
        Constant,
        Add,
        Sub,
        Mul,
        Div
    }

    [DebuggerDisplay("{Code} {Value}")]
    public class Node
    {
        public string Code;
        public bool HasValue;
        public long Value;
        public Operation Operation;
        public Node first;
        public Node second;
        public string firstCode;
        public string secondCode;

        public Node(string code, int value)
        {
            this.Code = code;
            this.Operation = Operation.Constant;
            this.Value = value;
            this.HasValue = true;
        }

        public Node(string code, Operation operation, string firstCode, string secondCode)
        {
            this.Code = code;
            this.Operation = operation;
            this.HasValue = false;
            this.Value = 0;
            this.firstCode = firstCode;
            this.secondCode = secondCode;
        }

        public bool CanCalculate()
        {
            return !HasValue && first.HasValue && second.HasValue;
        }

        public long Calculate()
        {
            var arg1 = first.Value;
            var arg2 = second.Value;

            switch (this.Operation)
            {
                case Operation.Add:
                    this.Value = arg1 + arg2;
                    break;
                case Operation.Sub:
                    this.Value = arg1 - arg2;
                    break;
                case Operation.Mul:
                    this.Value = arg1 * arg2;
                    break;
                case Operation.Div:
                    this.Value = arg1 / arg2;
                    break;
            }
            this.HasValue = true;
            return this.Value;
        }

        public bool DependsOn(Node target)
        {
            if (this == target)
            {
                return true;
            }

            if (this.Operation == Operation.Constant)
            {
                return false;
            }
            return this.first.DependsOn(target) || this.second.DependsOn(target);
        }

        protected bool Equals(Node other)
        {
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }

        public static bool operator ==(Node left, Node right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !Equals(left, right);
        }


    }
}