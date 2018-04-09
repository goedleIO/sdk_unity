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
    using UnityEngine.Networking;

    [TestFixture]
    public class GoedleTrackRequestTest
    {
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
        public GoedleHttpClient _gio_http_client;
        IGoedleDownloadBuffer _gdb = null;
        IGoedleUploadHandler _guh = null;
        IGoedleWebRequest _gw = null;

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
            _gio_http_client = (new GameObject("GoedleHTTPClient")).AddComponent<GoedleHttpClient>();
            _gdb = Substitute.For<IGoedleDownloadBuffer>();
            _guh = Substitute.For<IGoedleUploadHandler>();
            _gw = Substitute.For<IGoedleWebRequest>();

        }


        [Test]
        public void checkBehaviorGoedleWebRequestPOST()
        {
            _gio_object = new GoedleAnalytics(_api_key, _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, _staging);

            _gw.unityWebRequest.Returns(new UnityWebRequest());
            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);

            string authentification = "12345";
            _gio_http_client.sendPost(_url, authentification, _gw, _guh, _staging);
            _gw.Received(2).SendWebRequest();
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Content-Type"), Arg.Is<string>("application/json"));
            _gw.Received(1).SetRequestHeader(Arg.Is<string>("Authorization"), Arg.Is<string>("12345"));
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Authorization"), Arg.Any<string>());
        }

        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsIdentifyPOST()
        {
            _gio_object = new GoedleAnalytics(_api_key, _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, _staging);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));

            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);
            _gio_object.track(GoedleConstants.IDENTIFY, null, null, false, null, null, _guh);
            var result = JSON.Parse(stringContent);
            Assert.AreEqual(result["event"].Value, "identify");
            _gw.Received(2).SendWebRequest();
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Content-Type"), Arg.Is<string>("application/json"));
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Authorization"), Arg.Any<string>());
        }

        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsEventPOST()
        {
            _gio_object = new GoedleAnalytics(_api_key, _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, _staging);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));

            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);
            _gio_object.track("test", null, null, false, null, null, _guh);
            var result = JSON.Parse(stringContent);
            Assert.AreEqual(result["event"].Value, "test");
            _gw.Received(2).SendWebRequest();
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Content-Type"), Arg.Is<string>("application/json"));
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Authorization"), Arg.Any<string>());
        }

        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsEventIdPOST()
        {
            _gio_object = new GoedleAnalytics(_api_key, _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, _staging);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));

            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);
            _gio_object.track("test", "id", null, false, null, null, _guh);
            var result = JSON.Parse(stringContent);
            Assert.AreEqual(result["event"].Value, "test");
            Assert.AreEqual(result["event_id"].Value, "id");

            _gw.Received(2).SendWebRequest();
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Content-Type"), Arg.Is<string>("application/json"));
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Authorization"), Arg.Any<string>());
        }

        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsEventValuePOST()
        {
            _gio_object = new GoedleAnalytics(_api_key, _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, _staging);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));

            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);
            _gio_object.track("test", "id", "value", false, null, null, _guh);
            var result = JSON.Parse(stringContent);
            Assert.AreEqual(result["event"].Value, "test");
            Assert.AreEqual(result["event_id"].Value, "id");
            Assert.AreEqual(result["event_value"].Value, "value");

            _gw.Received(2).SendWebRequest();
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Content-Type"), Arg.Is<string>("application/json"));
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Authorization"), Arg.Any<string>());
        }

        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsGroupPOST()
        {
            _gio_object = new GoedleAnalytics(_api_key, _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, _staging);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));

            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);
            _gio_object.trackGroup("class", "school", _guh);
            var result = JSON.Parse(stringContent);
            Assert.AreEqual(result["event"].Value, "group");
            Assert.AreEqual(result["event_id"].Value, "class");
            Assert.AreEqual(result["event_value"].Value, "school");

            _gw.Received(2).SendWebRequest();
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Content-Type"), Arg.Is<string>("application/json"));
            _gw.Received(2).SetRequestHeader(Arg.Is<string>("Authorization"), Arg.Any<string>());
        }


        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsTrackStaging()
        {
            _gio_object = new GoedleAnalytics(_api_key, _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, true);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));

            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);
            _gio_object.trackGroup("class", "school", _guh);

            _gio_object.track("event", _guh);
            _gio_object.track("event", "event_id", _guh);
            _gio_object.track("event", "event_id", "50", _guh);
            _gio_object.set_user_id("test_user", _guh);

            _gw.Received(0).SendWebRequest();
        }

        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsNoTrackWhenApiKeyIsEmpty()
        {
            _gio_object = new GoedleAnalytics("", _app_key, _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, true);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));
            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);

            _gio_object.trackGroup("class", "school", _guh);
            _gio_object.track("event", _guh);
            _gio_object.track("event", "event_id", _guh);
            _gio_object.track("event", "event_id", "50", _guh);
            _gio_object.set_user_id("test_user", _guh);
            _gw.Received(0).SendWebRequest();

        }

        [Test]
        public void checkBehaviorGoedleWebRequestAnalyticsNoTrackWhenAppKeyIsEmpty()
        {
            _gio_object = new GoedleAnalytics(_api_key, "", _user_id, _app_version, _GA_TRACKIND_ID, _app_name, _GA_CD_1, _GA_CD_2, _GA_CD_EVENT, _gio_http_client, _gw, _guh, true);

            string stringContent = null;
            _guh.add(Arg.Do<string>(x => stringContent = x));

            _gw.isHttpError.Returns(false);
            _gw.isNetworkError.Returns(false);

            _gio_object.trackGroup("class", "school", _guh);
            _gio_object.track("event", _guh);
            _gio_object.track("event", "event_id", _guh);
            _gio_object.track("event", "event_id", "50", _guh);
            _gio_object.set_user_id("test_user", _guh);
            _gw.Received(0).SendWebRequest();

        }

    }
}