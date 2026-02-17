import React from "react";
import { Card, Col, Row, Table } from "react-bootstrap";

export default function PPDialogIssues({ issues, getLocalByVal, getLocal }) {
  function compare(a, b) {
    if (a.no < b.no) {
      return -1;
    }
    if (a.no > b.no) {
      return 1;
    }
    return 0;
  }
  return (
    <>
      {issues ? (
        <>
          <div id="showOnLargeScreen">
            <Table striped bordered hover size="sm" className="three-dtbl">
              <thead>
                <tr>
                  <th style={{ width: "50px" }}>{getLocal("No")}</th>
                  <th>{getLocal("Recommendation")}</th>
                  <th>{getLocal("Status")}</th>
                </tr>
              </thead>
              <tbody>
                {issues.length > 0 ? (
                  issues.sort(compare).map((issue, index) => (
                    <tr key={index}>
                      <td>{issue.no}</td>
                      <td>{issue.title}</td>
                      <td>
                        <strong>{issue.status}</strong>
                        {issue.implementationText ? (
                          <>
                            <br />
                            {issue.implementationText}
                          </>
                        ) : null}
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="3" className="text-center">
                      <strong>{getLocal("Empty List")}</strong>
                    </td>
                  </tr>
                )}
              </tbody>
            </Table>
          </div>
          <div id="showOnSmallScreen">
            {issues && issues.length > 0 ? (
              issues.sort(compare).map((issue, i) => (
                <Card key={i} className="card__margin-bottom">
                  <Card.Header>{issue.no} {issue.title}</Card.Header>
                  <Card.Body>
                    <Row>
                      <Col>{getLocal("No")}</Col>
                      <Col>{issue.no}</Col>
                    </Row>                    
                    <Row>
                      <Col>{getLocal("Recommendation")}</Col>
                      <Col>{issue.title}</Col>
                    </Row>
                    <Row>
                      <Col>{getLocal("Status")}</Col>
                      <Col><strong>{issue.status}</strong>
                        {issue.implementationText ? (
                          <>
                            <br />
                            {issue.implementationText}
                          </>
                        ) : null}</Col>
                    </Row>
                  </Card.Body>
                </Card>
              ))
            ) : (
              <Card className="card__margin-bottom">
                <strong>{getLocal("Empty List")}</strong>
              </Card>
            )}
          </div>
        </>
      ) : null}
    </>
  );
}
