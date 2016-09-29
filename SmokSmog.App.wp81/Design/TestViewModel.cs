namespace SmokSmog.Design
{
    public class TestViewModel
    {
        public int index0 => 0;

        private int index1 = 1;

        public int Index1
        {
            get { return index1; }
            set { index1 = value; }
        }

        private int index2 = 2;

        public int Index2
        {
            get { return index2; }
            set { index2 = value; }
        }
    }
}