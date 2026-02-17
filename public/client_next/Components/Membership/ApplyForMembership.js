import React from "react";
import { getAssociations } from "../../api/membership";
import { Card } from "react-bootstrap";
import Link from "next/link";

export default function ApplyForMembership({
  memberships,
  getLocal,
  getLocalByVal,
}) {
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

  return <>
    <button
      type="button"
      className="btn-sm btn btn-success"
      style={{ float: "none", margin: "5px 0px 0 10px" }}
      onClick={() => GetAssociationsForApply(2)}
    >
      {getLocal("Apply for Association Membership")}
    </button>
    {!memberships.find((m) => m.associationType == 1) ? (
      <button
        type="button"
        className="btn-sm btn btn-primary"
        style={{ float: "none", margin: "5px 0px 0 10px" }}
        onClick={() => GetAssociationsForApply(1)}
      >
        {getLocal("Apply for Chamber Membership")}
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
          {getLocal("Select")}{" "}
          {applyType == 2 ? getLocal("Association") : getLocal("Chamber")}
        </Card.Header>
        <Card.Body>
          <Card.Text>
            {associations.map((ass, index) => (
              <li key={index}
                style={{
                  whiteSpace: "nowrap",
                  overflow: "hidden",
                  maxWidth: "100%",
                }}
              >
                <Link
                  href={`/myServices/membershipApplyForm?associationId=${ass.id}&type=0`}
                  legacyBehavior>
                  {getLocalByVal(ass.nameEng, ass.nameLocal)}
                </Link>
              </li>
            ))}
            {!associations || associations.length <= 0 ? (
              <span style={{ color: "black" }}>
                {getLocal("Bank Setting Not Found")}
              </span>
            ) : null}
          </Card.Text>
        </Card.Body>
      </Card>
    ) : null}
  </>;
}
