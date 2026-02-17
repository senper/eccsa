import React from "react";
import { Card, Col, Row, Table } from "react-bootstrap";

export default function IssuesSummary({
  issues,
  issueTypes,
  getLocalByVal,
  getLocal,
}) {
  function compare(a, b) {
    if (a.nameEng < b.nameEng) {
      return -1;
    }
    if (a.nameEng > b.nameEng) {
      return 1;
    }
    return 0;
  }
  return (
    <>
      {issues && issueTypes ? (
        <>
          <div id="showOnLargeScreen">
            <Table striped bordered hover size="sm" className="three-dtbl">
              <thead>
                <tr>
                  <th>{getLocal("Issue Type")}</th>
                  <th style={{ width: "70px" }}>{getLocal("New")}</th>
                  <th style={{ width: "70px" }}>{getLocal("Assigned")}</th>
                  <th style={{ width: "70px" }}>{getLocal("Resolved")}</th>
                </tr>
              </thead>
              <tbody>
                {issueTypes.length > 0 && issues.length > 0 ? (
                  issueTypes.sort(compare).map((type, index) => {
                    let curIssues = issues.filter(
                      (s) => s.type == type.nameEng
                    );
                    let newCount = curIssues.find((i) => i.status == "New");
                    let assignedCount = curIssues.find(
                      (i) => i.status == "Assigned"
                    );
                    let resolvedCount = curIssues.find(
                      (i) => i.status == "Resolved"
                    );
                    if (curIssues.length > 0) {
                      return (
                        <tr key={index}>
                          <td>{type.name}</td>
                          <td>{newCount ? newCount.formattedCount : 0}</td>
                          <td>
                            {assignedCount ? assignedCount.formattedCount : 0}
                          </td>
                          <td>
                            {resolvedCount ? resolvedCount.formattedCount : 0}
                          </td>
                        </tr>
                      );
                    }
                  })
                ) : (
                  <tr>
                    <td colSpan="4" className="text-center">
                      <strong>{getLocal("No Issues")}</strong>
                    </td>
                  </tr>
                )}
              </tbody>
            </Table>
          </div>
          <div id="showOnSmallScreen">
            {issueTypes.length > 0 && issues.length > 0 ? (
              issueTypes.sort(compare).map((type, index) => {
                let curIssues = issues.filter((s) => s.type == type.nameEng);
                let newCount = curIssues.find((i) => i.status == "New");
                let assignedCount = curIssues.find(
                  (i) => i.status == "Assigned"
                );
                let resolvedCount = curIssues.find(
                  (i) => i.status == "Resolved"
                );
                if (curIssues.length > 0) {
                  return (
                    <Card key={index} className="card__margin-bottom">
                      <Card.Header>{type.name}</Card.Header>
                      <Card.Body>
                        <Row>
                          <Col>{getLocal("New")}</Col>
                          <Col>{newCount ? newCount.formattedCount : 0}</Col>
                        </Row>
                        <Row>
                          <Col>{getLocal("Assigned")}</Col>
                          <Col>
                            {assignedCount ? assignedCount.formattedCount : 0}
                          </Col>
                        </Row>
                        <Row>
                          <Col>{getLocal("Resolved")}</Col>
                          <Col>
                            {resolvedCount ? resolvedCount.formattedCount : 0}
                          </Col>
                        </Row>
                      </Card.Body>
                    </Card>
                  );
                }
              })
            ) : (
              <Card className="card__margin-bottom">
                <strong>{getLocal("No Issues")}</strong>
              </Card>
            )}
          </div>
        </>
      ) : null}
    </>
  );
}
