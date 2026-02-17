import Link from "next/link";
import React from "react";
import { Card } from "react-bootstrap";
import { getPathFromTitle } from "../../pages/api/menu";

export default function ResourceLinks({ resourceTypes, getLocal, getLocalByVal }) {
  return (
    <Card text="white" style={{ width: "18rem" }}>
      <Card.Header>{getLocal("Resources Links")}</Card.Header>
      <Card.Body>
        <Card.Text>
          {resourceTypes.map((res, index) => (
            <li key={index}>
              <Link href={`/eresources/${getPathFromTitle(res.name)}`} legacyBehavior>
                {getLocal(res.name)}
              </Link>
            </li>
          ))}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
