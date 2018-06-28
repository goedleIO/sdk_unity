#define TESTING

namespace goedle_sdk.detail
{
    using UnityEngine;
    using NUnit.Framework;
    using NSubstitute;
    using System.Linq;
    using System.Collections;
    using UnityEngine.TestTools;
    using SimpleJSON;

    [TestFixture]
    public class GoedleWebRequestTest {
        public string _app_key;
        public string _api_key;
        public string _user_id;
        public string _app_version;
        public string _app_name;
        public int _ts;
        public int _timezone;
        public string _event_name;
        public string _event_id;
        public string _event_value;
        public string _trait_key;
        public string _trait_value;
        public string _GA_TRACKIND_ID;
        public bool _ga_active;
        public int _GA_CD_1;
        public int _GA_CD_2;
        public string _GA_CD_EVENT;
        public string _anonymous_id;
        public GoedleAnalytics _gio_object;
        public string _strategy_string = "{\"config\": { \"scenario\": \"seashore\" , \"wind_speed\": \"fast\"}, \"id\":1}";
        public string _url = null;
        public bool _staging = false;
		public bool _content = true;

        public GoedleHttpClient _gio_http_client = (new GameObject("GoedleHTTPClient")).AddComponent<GoedleHttpClient>();
        IGoedleDownloadBuffer _gdb = null;
        IGoedleWebRequest _gw = null;
        IGoedleUploadHandler _guh = null;

        [SetUp]
        public void SetUp()
        {
            _app_key = "test_app_key";
            _user_id = "u1";
            _app_version = "v_test";
            _ts = 0;
            _app_name = "test_app";
            _timezone = 360000;
            _event_name = "test_event";
            _event_id = null;
            _event_value = null;
            _ga_active = false;
            _anonymous_id = null;
            _trait_key = null;
            _trait_value = null;
            _api_key = "test_api_key";
            _GA_TRACKIND_ID = null;
            _GA_CD_1 = 0;
            _GA_CD_2 = 0;
            _GA_CD_EVENT = "group";
            _url = "test_url";
            _staging = false;
			_content = true;

            _gw = Substitute.For<IGoedleWebRequest>();
            _guh = Substitute.For<IGoedleUploadHandler>();
            _gdb = Substitute.For<IGoedleDownloadBuffer>();
            _gio_object = new GoedleAnalytics(
                    _api_key, 
                    _app_key, 
                    _user_id, 
                    _app_version,
                    _GA_TRACKIND_ID, 
                    _app_name, 
                    _GA_CD_1, 
                    _GA_CD_2, 
                    _GA_CD_EVENT, 
                    _gio_http_client , 
                    _gw, 
                    _guh, 
                    _staging,
				    _content);
        }

        [UnityTest]
        public IEnumerator checkBehaviorGoedleWebRequest(){
            _gdb.text.Returns(_strategy_string);
            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);
            _gio_http_client.requestStrategy(_url, _gio_object, _gw, _gdb, _staging);

            int c = 0;
            while (_gio_object.strategy == null || c < 120)
            {
                yield return null;
                c++;
            }

            _gw.Received(2).SendWebRequest();

            Assert.AreEqual(_gio_object.strategy["config"]["scenario"].Value,"seashore");
            Assert.AreEqual(_gio_object.strategy["config"]["wind_speed"].Value, "fast");
            Assert.AreEqual(_gio_object.strategy["id"].Value, "1" );
        }

        [UnityTest]
        public IEnumerator checkBehaviorGoedleWebRequestHttpError()
        {
            _gdb.text.Returns(_strategy_string);
            _gw.isHttpError.Returns(true);
            _gw.isNetworkError.Returns(false);
            _gio_http_client.requestStrategy(_url, _gio_object, _gw, _gdb, _staging);

            int c = 0;
            while (_gio_object.strategy == null || c < 120)
            {
                yield return null;
                c++;
            }
            _gw.Received(2).SendWebRequest();
            Assert.AreEqual(_gio_object.strategy["error"]["isHttpError"].Value, "True");
            Assert.AreEqual(_gio_object.strategy["error"]["isNetworkError"].Value, "False");
        }

        [UnityTest]
        public IEnumerator checkBehaviorGoedleWebRequestNetworkError()
        {
            _gdb.text.Returns(_strategy_string);
            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(true);
            _gio_http_client.requestStrategy(_url, _gio_object, _gw, _gdb, _staging);
            int c = 0;
            while (_gio_object.strategy == null || c < 120)
            {
                yield return null;
                c++;
            }
            _gw.Received(2).SendWebRequest();

            Assert.AreEqual(_gio_object.strategy["error"]["isHttpError"].Value, "False");
            Assert.AreEqual(_gio_object.strategy["error"]["isNetworkError"].Value, "True");
        }

        [UnityTest]
        public IEnumerator checkBehaviorGoedleWebRequestNetworkErrorHttpError()
        {
            _gdb.text.Returns(_strategy_string);
            _gw.isHttpError.Returns(true);
            _gw.isNetworkError.Returns(true);
            _gio_http_client.requestStrategy(_url, _gio_object, _gw, _gdb, _staging);

            int c = 0;
            while (_gio_object.strategy == null || c < 120)
            {
                yield return null;
                c++;
            }
            _gw.Received(2).SendWebRequest();

            Assert.AreEqual(_gio_object.strategy["error"]["isHttpError"].Value, "True");
            Assert.AreEqual(_gio_object.strategy["error"]["isNetworkError"].Value, "True");
        }
    }
}