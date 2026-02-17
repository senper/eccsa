import React from "react";

import { Link } from "react-router-dom";
import { Card } from "react-bootstrap";

export default function AssociationLinks({
  associations,
  loadAssociation,
  selectedAssociation}) {
  return (
    <Card text="white" style={{ width: "18rem" }}>
      <Card.Header>Members</Card.Header>
      <Card.Body>
        <Card.Text>
          {associations.map((ass) => (
            <li style={{whiteSpace: "nowrap", overflow: "hidden", maxWidth: "270px"}}>
              <Link title={ass.nameEng} 
              style={selectedAssociation && selectedAssociation.id == ass.id ? {color: "black"} : {}}
                onClick={() => loadAssociation(ass.id)}
              >
                {ass.shortName}
              </Link>
            </li>
          ))}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
