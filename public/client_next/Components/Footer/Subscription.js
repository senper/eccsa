import { AddAlert } from "@mui/icons-material";
import { Snackbar } from "@mui/material";
import React from "react";
import { Col, Row } from "react-bootstrap";
import { getSubscriptions, saveSubscription } from "../../api/subscriptions";
import { getLookupValues } from "../../pages/api/contactUs";

export default function Subscription() {
  const tokenMain =
    typeof window !== "undefined" ? localStorage.getItem("access_token") : null;
  const [token, setToken] = React.useState(null);
  React.useEffect(() => {
    setToken(tokenMain);
  }, [tokenMain]);
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  const [topics, setTopics] = React.useState([]);
  const [subscriptions, setSubscriptions] = React.useState([]);
  const [subscriptionLoaded, setSubscriptionLoaded] = React.useState([]);

  React.useEffect(() => {
    if (!token) {
      setSubscriptions([]);
      return;
    }
    getLookupValues("00000000-0000-0000-0000-00000000002f").then((data) => {
      setTopics(data.data.$values);
      loadSubscriptions();
    });
  }, [token]);
  const loadSubscriptions = (id) => {
    getSubscriptions().then((data) => {
      if (data.data) {
        setSubscriptionLoaded(true);
        setSubscriptions(data.data.$values);
      } else {
        setSubscriptionLoaded(false);
      }
    });
  };
  function compare(a, b) {
    if (a.name < b.name) {
      return -1;
    }
    if (a.name > b.name) {
      return 1;
    }
    return 0;
  }
  const [topicSubscriptionStatus, setTopicSubscriptionStatus] = React.useState(
    []
  );
  function updateSubscriptionStatus(topicId, checked) {
    const newStatus = [...topicSubscriptionStatus];
    if (newStatus.findIndex((s) => s.key == topicId) >= 0) {
      newStatus.find((s) => s.key == topicId).value = checked;
    } else {
      newStatus.push({ key: topicId, value: checked });
    }
    setTopicSubscriptionStatus(newStatus);
  }

  function getChecked(topicId) {
    var status = topicSubscriptionStatus.find((s) => s.key == topicId);
    if (status) {
      return status.value;
    }
    return subscriptions.findIndex((s) => s.topicId == topicId) >= 0;
  }
  function renderTopics() {
    return topics
      ? topics.sort(compare).map((topic, index) => (
          <Row key={index}>
            <Col>
              <input
                type="checkbox"
                name={topic.id}
                checked={getChecked(topic.id)}
                onChange={(event) =>
                  updateSubscriptionStatus(topic.id, event.target.checked)
                }
              />{" "}
              {topic.name}
            </Col>
          </Row>
        ))
      : null;
  }
  function updateSubscription() {
    const data = [];
    topicSubscriptionStatus.forEach((s) => {
      data.push({ TopicId: s.key, Status: s.value });
    });
    saveSubscription(data)
      .then((data) => {
        if (data.data.saved) {
          setErrorMsg("Saved!!");
          showNotification("tr");
          loadSubscriptions();
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        // console.log(error);
        showNotification("tr");
      });
  }
  return token && topics && subscriptionLoaded ? (
    <div
      className="col-lg-3 col-md-3 mb-4 mb-md-0"
      style={{ textAlign: "left" }}
    >
      <h5 className="text-uppercase">Subscriptions</h5>
      {renderTopics()}
      <Row>
        <Col className="text-center">
          <button
            type="button"
            className="btn btn-success btn-sm m-0"
            style={{ float: "left" }}
            onClick={() => updateSubscription()}
          >
            Update
          </button>
        </Col>
      </Row>
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : "Failed to Save"}
        open={error}
        onClose={() => setError(false)}
      />
    </div>
  ) : (
    <>
      {/* {storedSubscriptions ? store.dispatch(clearSubscriptions()) : null}
      {storedTopics ? store.dispatch(clearTopics()) : null} */}
    </>
  );
}
