import { AddAlert } from "@material-ui/icons";
import { Snackbar } from "@mui/material";
import React from "react";
import { Col, Row } from "react-bootstrap";
//import { useSelector } from "react-redux";
import {
  clearSubscriptions,
  clearTopics,
  //storeSubscriptions,
  //storeTopics,
} from "../../actions/subscriptionActions";
import { getLookupValues } from "../../services/contactUs";
import {
  getSubscriptions,
  saveSubscription,
} from "../../services/subscriptions";
//import store from "../../store";

export default function Subscription() {
  var token = localStorage.getItem("access_token");

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
  //const { topics: storedTopics, subscriptions: storedSubscriptions } =
 //   useSelector((state) => state.topicSubscriptions);
 // console.log(storedTopics, storedSubscriptions);
  const [topics, setTopics] =React.useState([]);// React.useState(storedTopics ? storedTopics : []);
  const [subscriptions, setSubscriptions] = React.useState([]);
  /*  React.useState(
    storedSubscriptions ? storedSubscriptions : []
  ); */
  React.useEffect(() => {
    if(!token){
      setSubscriptions([]);
      return;
    } 
    //if (!storedTopics) {
      getLookupValues("00000000-0000-0000-0000-00000000002f").then((data) => {
        setTopics(data.data.$values);
        //store.dispatch(storeTopics(data.data.$values));
        //console.log("topics load");
        loadSubscriptions();
      });
   // }

   // if (!storedSubscriptions) {
   //   loadSubscriptions();
  //  }
  }, [token]);
  const loadSubscriptions = (id) => {
    getSubscriptions().then((data) => {
      setSubscriptions(data.data.$values);
      //store.dispatch(storeSubscriptions(data.data.$values));
      //console.log("subs load");
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
      ? topics.sort(compare).map((topic) => (
          <Row>
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
        console.log(error);
        showNotification("tr");
      });
  }
  return token && topics ? (
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
            class="btn btn-success btn-sm m-0"
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
        closeNotification={() => setError(false)}
        close
      />
    </div>
  ) : (
    <>
      {/* {storedSubscriptions ? store.dispatch(clearSubscriptions()) : null}
      {storedTopics ? store.dispatch(clearTopics()) : null} */}
    </>
  );
}
