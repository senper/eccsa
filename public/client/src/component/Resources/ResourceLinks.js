import React from "react";

import { Link } from "react-router-dom";

import { Card } from "react-bootstrap";
import { resourceTypes } from "../../services/resources";

export default function ResourceLinks() {
  return (
    <Card text="white" style={{ width: "18rem" }}>
      <Card.Header>Resources Links</Card.Header>
      <Card.Body>
        <Card.Text>
          {resourceTypes.map((res) => (
            <li>
              <Link
                to={{
                  pathname: "/eresources",
                  state: {
                    typeId: res.value,
                    typeText: res.text,
                  },
                }}
                eventKey="eresources"
              >
                {res.text}
              </Link>
            </li>
          ))}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
