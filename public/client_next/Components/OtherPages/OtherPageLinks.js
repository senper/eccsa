import Link from "next/link";
import React from "react";
import { Card } from "react-bootstrap";

export default function OtherPageLinks({
  getLocal,
  links,
  title,
  loadQLContents,
  loading,
}) {
  return (
    <Card text="white" style={{ width: "18rem" }}>
      <Card.Header>{loading ? `${getLocal("Loading")}...` : title}</Card.Header>
      <Card.Body>
        <Card.Text>
          {loading
            ? `${getLocal("Loading")}...`
            : links.map((ql, index) => (
                <li key={index}>
                  {ql.path ? (
                    <Link href={`${ql.path}`} legacyBehavior>
                      {ql.title}
                    </Link>
                  ) : (
                    <a className="text-primary" onClick={() => loadQLContents(ql.id)}>
                      {ql.title}
                    </a>
                  )}
                </li>
              ))}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
