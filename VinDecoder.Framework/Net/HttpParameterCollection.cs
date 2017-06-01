using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VinDecoder.Framework.Net {
    public class HttpParameterCollection : IEnumerable<KeyValuePair<string, string>> {
        private IDictionary<string, List<string>> m_paramDict;

        public HttpParameterCollection(bool sort = true) {
            if (sort) {
                m_paramDict = new SortedDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            } else {
                m_paramDict = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            }
        }

        public HttpParameterCollection()
            : this(true) { }

        public HttpParameterCollection Add(string key, string value) {
            if (!m_paramDict.ContainsKey(key)) {
                m_paramDict.Add(key, new List<string>());
            }

            m_paramDict[key].Add(value);

            return this;
        }

        public IEnumerable<string> Keys {
            get { return m_paramDict.Keys; }
        }

        public HttpParameterCollection Remove(string key) {
            m_paramDict.Remove(key);

            return this;
        }

        public bool IsEmpty {
            get { return m_paramDict.Count == 0; }
        }

        public HttpParameterCollection Clear() {
            m_paramDict.Clear();

            return this;
        }

        public string ToQueryString(Encoding encoding = null) {
            return ToString(x => HttpUtility.UrlEncode(x, encoding ?? Encoding.UTF8));
        }

        public override string ToString() {
            return ToString(null);
        }

        public string ToString(Func<string, string> encode) {
            var parameterString = new StringBuilder();

            foreach (var kv in this) {
                string key = encode == null ? kv.Key : encode(kv.Key);
                string value = encode == null ? kv.Value : encode(kv.Value);
                parameterString.Append(key);
                parameterString.Append("=");
                parameterString.Append(value);
                parameterString.Append("&");
            }

            if (parameterString[parameterString.Length - 1] == '&') {
                parameterString.Remove(parameterString.Length - 1, 1);
            }

            return parameterString.ToString();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            foreach (var kv in m_paramDict) {
                if (kv.Value == null) {
                    yield return new KeyValuePair<string, string>(kv.Key, "");
                } else {
                    foreach (var v in kv.Value) {
                        yield return new KeyValuePair<string, string>(kv.Key, v);
                    }
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
