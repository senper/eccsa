import React from "react";
import { Card } from "react-bootstrap";

export default function AssociationLinks({
  associations,
  loadAssociation,
  selectedAssociation,
  loadingList,
  getLocal,
  getLocalByVal,
}) {
  return (
    <Card style={{ width: "18rem" }}>
      <Card.Header>{getLocal("Members")}</Card.Header>
      <Card.Body>
        <Card.Text>
          {loadingList ? `${getLocal("Loading")} ...` : null}
          {associations.map((ass, index) => (
            <li
              key={index}
              style={{
                whiteSpace: "nowrap",
                overflow: "hidden",
                maxWidth: "270px",
                listStyle: "none",
              }}
            >
              <a
                href="#"
                title={getLocalByVal(ass.nameEng, ass.nameLocal)}
                style={
                  selectedAssociation && selectedAssociation.id == ass.id
                    ? { color: "black" }
                    : {}
                }
                onClick={() => loadAssociation(ass.id)}
              >
                {getLocalByVal(ass.shortName, ass.shortName_Local)}
              </a>
            </li>
          ))}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
