using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VinDecoder.Framework.Net {
    public class HttpCaller {

        public const int DefaultTimeout = 30000;

        public string Get(string url, HttpParameterCollection parameters, int timeout = DefaultTimeout, Encoding paramsEncode = null, Encoding contentEncoding = null, Action<HttpWebRequest> updateRequest = null, CookieContainer cookie = null) {
            Guards.ThrowIfIsNullOrWhiteSpace(url, "url");

            timeout = timeout < 1 ? DefaultTimeout : timeout;
            contentEncoding = contentEncoding ?? Encoding.UTF8;
            updateRequest = updateRequest ?? (x => { });


            string reqUrl = string.Empty;
            if (parameters != null && !parameters.IsEmpty) {
                reqUrl = string.Format("{0}?{1}", url, parameters.ToQueryString(paramsEncode));
            } else {
                reqUrl = url;
            }

            HttpWebRequest req = null;
            HttpWebResponse rep = null;
            Stream repStream = null;
            StreamReader repReader = null;
            string ret = null;

            try {
                req = (HttpWebRequest)HttpWebRequest.Create(reqUrl);
                req.Timeout = timeout;
                req.ReadWriteTimeout = timeout;
                req.AllowAutoRedirect = false;

                updateRequest(req);

                if (cookie != null) {
                    req.CookieContainer = cookie;
                }

                rep = (HttpWebResponse)req.GetResponse();

                repStream = rep.GetResponseStream();

                Encoding repEnc = null;
                if (string.IsNullOrEmpty(rep.CharacterSet)) {
                    repEnc = contentEncoding;
                } else {
                    repEnc = Encoding.GetEncoding(rep.CharacterSet);
                }

                repReader = new StreamReader(repStream, repEnc);
                ret = repReader.ReadToEnd();

                return ret;
            } finally {
                if (null != rep) {
                    rep.Close();
                }

                if (null != repReader) {
                    repReader.Close();
                }
            }
        }

        public byte[] GetRaw(string url, HttpParameterCollection parameters, int timeout = DefaultTimeout, Encoding paramsEncode = null, Encoding contentEncoding = null, Action<HttpWebRequest> updateRequest = null, CookieContainer cookie = null) {
            Guards.ThrowIfIsNullOrWhiteSpace(url, "url");

            timeout = timeout < 1 ? DefaultTimeout : timeout;
            contentEncoding = contentEncoding ?? Encoding.UTF8;
            updateRequest = updateRequest ?? (x => { });

            string reqUrl = string.Empty;
            if (null != parameters && !parameters.IsEmpty) {
                reqUrl = string.Format("{0}?{1}", url, parameters.ToQueryString(paramsEncode));
            } else {
                reqUrl = url;
            }

            HttpWebRequest req = null;
            WebResponse rep = null;
            Stream repStream = null;

            try {
                req = (HttpWebRequest)HttpWebRequest.Create(reqUrl);
                req.Timeout = timeout;
                req.ReadWriteTimeout = timeout;
                req.AllowAutoRedirect = false;

                updateRequest(req);

                if (cookie != null) {
                    req.CookieContainer = cookie;
                }

                rep = req.GetResponse();
                repStream = rep.GetResponseStream();

                using (var mStream = new MemoryStream()) {
                    repStream.CopyTo(mStream);
                    return mStream.ToArray();
                }
            } finally {
                if (null != rep) {
                    rep.Close();
                }
            }
        }

        public string Post(string url, HttpParameterCollection parameters, int timeout = DefaultTimeout, Encoding paramsEncode = null, Encoding contentEncoding = null, Action<HttpWebRequest> updateRequest = null, CookieContainer cookie = null) {
            Guards.ThrowIfIsNullOrWhiteSpace(url, "url");

            var data = parameters == null ? string.Empty : parameters.ToQueryString(paramsEncode);
            timeout = timeout < 1 ? DefaultTimeout : timeout;
            contentEncoding = contentEncoding ?? Encoding.UTF8;
            updateRequest = updateRequest ?? (x => { });


            HttpWebRequest req = null;
            HttpWebResponse rep = null;
            string postStr = null;
            byte[] postData = null;
            Stream reqStream = null;
            Stream repStream = null;
            StreamReader sReader = null;
            string ret = null;

            try {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Timeout = timeout;
                req.ReadWriteTimeout = timeout;
                req.AllowAutoRedirect = true;
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                updateRequest(req);

                if (cookie != null) {
                    req.CookieContainer = cookie;
                }

                postStr = parameters.ToQueryString();
                postData = contentEncoding.GetBytes(postStr);
                req.ContentLength = postData.Length;

                reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Flush();

                rep = (HttpWebResponse)req.GetResponse();
                repStream = rep.GetResponseStream();

                Encoding repEnc = null;

                if (string.IsNullOrEmpty(rep.CharacterSet)) {
                    repEnc = contentEncoding;
                } else {
                    repEnc = Encoding.GetEncoding(rep.CharacterSet);
                }

                sReader = new StreamReader(repStream, repEnc);
                ret = sReader.ReadToEnd();

                return ret;
            } finally {
                if (null != rep) {
                    rep.Close();
                }

                if (null != sReader) {
                    sReader.Close();
                }
            }
        }

        public string Post(string url, string body, int timeout = DefaultTimeout, Encoding paramsEncode = null, Encoding contentEncoding = null, Action<HttpWebRequest> updateRequest = null, CookieContainer cookie = null) {
            Guards.ThrowIfIsNullOrWhiteSpace(url, "url");

            timeout = timeout < 1 ? DefaultTimeout : timeout;
            contentEncoding = contentEncoding ?? Encoding.UTF8;
            updateRequest = updateRequest ?? (x => { });


            HttpWebRequest req = null;
            HttpWebResponse rep = null;
            string postStr = null;
            byte[] postData = null;
            Stream reqStream = null;
            Stream repStream = null;
            StreamReader sReader = null;
            string ret = null;

            try {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Timeout = timeout;
                req.ReadWriteTimeout = timeout;
                req.AllowAutoRedirect = true;
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                updateRequest(req);

                if (cookie != null) {
                    req.CookieContainer = cookie;
                }

                postStr = body ?? string.Empty;
                postData = contentEncoding.GetBytes(postStr);
                req.ContentLength = postData.Length;

                reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Flush();

                rep = (HttpWebResponse)req.GetResponse();
                repStream = rep.GetResponseStream();

                Encoding repEnc = null;

                if (string.IsNullOrEmpty(rep.CharacterSet)) {
                    repEnc = contentEncoding;
                } else {
                    repEnc = Encoding.GetEncoding(rep.CharacterSet);
                }

                sReader = new StreamReader(repStream, repEnc);
                ret = sReader.ReadToEnd();

                return ret;
            } finally {
                if (null != rep) {
                    rep.Close();
                }

                if (null != sReader) {
                    sReader.Close();
                }
            }
        }

        public byte[] PostRaw(string url, HttpParameterCollection parameters, int timeout = DefaultTimeout, Encoding paramsEncode = null, Encoding contentEncoding = null, Action<HttpWebRequest> updateRequest = null, CookieContainer cookie = null) {
            Guards.ThrowIfIsNullOrWhiteSpace(url, "url");

            var data = parameters == null ? string.Empty : parameters.ToQueryString(paramsEncode);
            timeout = timeout < 1 ? DefaultTimeout : timeout;
            contentEncoding = contentEncoding ?? Encoding.UTF8;
            updateRequest = updateRequest ?? (x => { });


            HttpWebRequest req = null;
            HttpWebResponse rep = null;
            string postStr = null;
            byte[] postData = null;
            Stream reqStream = null;
            Stream repStream = null;
            StreamReader sReader = null;

            try {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Timeout = timeout;
                req.ReadWriteTimeout = timeout;
                req.AllowAutoRedirect = true;
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                updateRequest(req);

                if (cookie != null) {
                    req.CookieContainer = cookie;
                }

                postStr = parameters.ToQueryString();
                postData = contentEncoding.GetBytes(postStr);
                req.ContentLength = postData.Length;

                reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Flush();

                rep = (HttpWebResponse)req.GetResponse();
                repStream = rep.GetResponseStream();

                using (var mStream = new MemoryStream()) {
                    repStream.CopyTo(mStream);
                    return mStream.ToArray();
                }
            } finally {
                if (null != rep) {
                    rep.Close();
                }

                if (null != sReader) {
                    sReader.Close();
                }
            }
        }
    }
}
