import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import { Link } from "react-router-dom";
import { getAssociations } from "../../services/membership";
import { Card } from "react-bootstrap";

export default function ApplyForMembership({ memberships }) {
  const token = localStorage.getItem("access_token");
  const [associations, setAssociations] = React.useState([]);
  const [applyForAssociation, setApplyForAssociation] = React.useState(false);
  const [applyType, setApplyType] = React.useState(false);

  function GetAssociationsForApply(type) {
    if (
      (!applyForAssociation && associations.length <= 0) ||
      type != applyType
    ) {
      setAssociations([]);
      getAssociations(type).then((data) => {
        setAssociations(data.data.$values);
      });
    }
    setApplyForAssociation(type != applyType ? true : !applyForAssociation);
    setApplyType(type);
  }

  return (
    <>
      <button
        type="button"
        class="btn-sm btn btn-success"
        style={{ float: "none", margin: "5px 0px 0 10px" }}
        onClick={() => GetAssociationsForApply(2)}
      >
        Apply for Association Membership
      </button>
      {!memberships.find((m) => m.associationType == 1) ? ( 
      <button
        type="button"
        class="btn-sm btn btn-primary"
        style={{ float: "none", margin: "5px 0px 0 10px" }}
        onClick={() => GetAssociationsForApply(1)}
      >
        Apply for Chamber Membership
      </button>
       ) : null} 
      {applyForAssociation ? (
        <Card
          text="white"
          style={{ maxWidth: "600px", margin: "5px 0px 0 10px" }}
        >
          <Card.Header
            style={{
              backgroundColor: applyType == 2 ? "#157347" : "#0b5ed7",
            }}
          >
            Select {applyType == 2 ? "Association" : "Chamber"}
          </Card.Header>
          <Card.Body>
            <Card.Text>
              {associations.map((ass) => (
                <li
                  style={{
                    whiteSpace: "nowrap",
                    overflow: "hidden",
                    maxWidth: "100%",
                  }}
                >
                  <Link
                    to={{
                      pathname: "/membershipApplicationForm",
                      state: {
                        associationId: ass.id,
                        type: 0,
                      },
                    }}
                  >
                    {ass.nameEng}
                  </Link>
                </li>
              ))}
             {!associations || associations.length <= 0 ? <p style={{color: "black"}}>Bank Setting Not Found</p> : null}
            </Card.Text>
          </Card.Body>
        </Card>
      ) : null}
    </>
  );
}
