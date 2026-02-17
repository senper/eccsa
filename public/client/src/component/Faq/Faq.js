import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./Faq.css";
import { Button, Col, Row } from "react-bootstrap";
import { useHistory } from "react-router-dom";
import { getSectorsList } from "../../services/businessInterest";
import { getBusinessInformationList } from "../../services/businessInformation";
const toasts = [
  {
    id: "Networking",
    category: "Networking",
    message:
      "To find local and foreign markets for their products and  services.",
  },
  {
    id: "Capacity",
    category: "Capacity Building",
    message:
      "in the area of training, manual preparation, strategic plan document designing, and the like)",
  },
  {
    category: "Consultancy",
    message: "membership and business development consultancy",
  },
  {
    category: "Experience Sharing",
    message: "exchange information and share experience etc.",
  },
];

function Toast(props) {
  return (
    <div className={`Toast Toast--${props.category}`}>
      <main className="Toast__message">
        <header className="Toast__message-category">{props.category}</header>
        <p className="Toast__message-text">{props.message}</p>
      </main>
      <button
        className="Toast__button"
        type="button"
        onClick={() => props.dismiss(props.id)}
      ></button>
    </div>
  );
}
export default function Faq() {
  const history = useHistory();
  var token = localStorage.getItem("access_token");
  const [sectors, setSectors] = React.useState([]);
  React.useEffect(() => {
    getSectorsList(true).then((data) => {
      if (data.data.sectors.$values) {
        setSectors(data.data.sectors.$values.filter((s) => !s.parentId));
      }
    });
  }, []);
  
  const [businessInfoList, setBusinessInfoList] = React.useState([]);
  React.useEffect(() => {
    getBusinessInformationList().then((data) => {
      setBusinessInfoList(data.data.$values);
    });
  }, []);
  function becomeAMember() {
    return token
      ? history.push({
          pathname: "/myServices",
          state: {
            eventKey: "myServices",
            navTo: "membership",
          },
        })
      : history.push({
          pathname: "/login",
        });
  }
  function addBusinessInterest() {
    return token
      ? history.push({
          pathname: "/myServices",
          state: {
            eventKey: "myServices",
            navTo: "businessInterest",
          },
        })
      : history.push({
          pathname: "/login",
        });
  }
  return (
    <>
      <Row>
        <Col md={6}>
          <div class="row">
            <div class="bg">Membership</div>
          </div>
          {toasts.map((toast, i) => (
            <Toast category={toast.category} key={i} message={toast.message} />
          ))}
          <Button
            type="submit"
            style={{
              float: "none",
              marginTop: "0",
              marginBottom: "0",
              marginLeft: "40%",
            }}
            variant="outline-secondary"
            size="sm"
            id="button-addon2"
            onClick={() => becomeAMember()}
          >
            Become A Member
          </Button>
        </Col>
        <Col md={6}>
          <div class="row">
            <div class="bg">Business Interests In</div>
          </div>
          <div style={{ maxHeight: "145px", overflowY: "scroll" }}>
            {sectors.map((s) => (
              <>
                <Button
                  style={{
                    marginTop: "0",
                    marginBottom: "10px",
                    width: "100%",
                  }}
                  variant="outline-secondary"
                  id="button-addon2"
                  onClick={() => {
                    history.push({
                      pathname: "/matchMaking",
                      state: {
                        eventKey: "MatchMaking",
                        sectorId: s.id,
                      },
                    });
                  }}
                >
                  {s.name}
                </Button>
                <br />
              </>
            ))}
          </div>
          <Button
            type="submit"
            style={{
              float: "none",
              marginTop: "0",
              marginBottom: "0",
              marginLeft: "34%",
            }}
            variant="outline-secondary"
            size="sm"
            id="button-addon2"
            onClick={() => addBusinessInterest()}
          >
            Add Business Interest
          </Button>
          
          <div class="row">
            <div class="bg">Business Information</div>
          </div>
          <div style={{ maxHeight: "185px", overflowY: "scroll" }}>
            {businessInfoList.map((s) => (
              <>
                <Button
                  style={{
                    marginTop: "0",
                    marginBottom: "10px",
                    width: "100%",
                  }}
                  variant="outline-secondary"
                  id="button-addon2"
                  onClick={() => {
                    history.push({
                      pathname: "/businessInformation",
                      state: {
                        eventKey: "businessInformation",
                        newsId: s.id,
                      },
                    });
                  }}
                >
                  {s.title}
                </Button>
                <br />
              </>
            ))}
          </div>
        </Col>
      </Row>
    </>
  );
}
