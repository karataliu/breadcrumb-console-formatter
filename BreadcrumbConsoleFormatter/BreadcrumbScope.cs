using System.Collections;

namespace Util.Extensions.Logging
{
    public class BreadcrumbScope : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> dic = new();
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => dic.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public BreadcrumbScope Add(string key, string value)
        {
            dic[key] = value;
            return this;
        }
    }
}
