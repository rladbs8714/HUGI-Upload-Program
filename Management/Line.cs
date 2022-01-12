using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management
{
    /// <summary>
    /// 입력될 한 줄의 정보를 담고 있음.
    /// 예) ①방문일시 : 11.17
    /// </summary>
    public class Line
    {
        public bool Empty = true;
        public int key;
        public string ResultValue => leftValue + rightValue;

        private readonly string leftValue;
        private readonly string rightValue;


        public Line(int _key, string _leftValue, string _rightValue)
        {
            key = _key;
            leftValue = _leftValue;
            rightValue = _rightValue;

            if(_leftValue.Any() && _rightValue.Any())
            {
                Empty = false;
            }
        }
    }
}
