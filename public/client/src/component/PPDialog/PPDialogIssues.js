import React from "react";
import { Table } from "react-bootstrap";
import "../ServiceRequest/ServiceRequestDetailTbl.css";

export default function PPDialogIssues({ issues }) {
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
          <Table striped bordered hover size="sm" className="three-dtbl">
            <thead>
              <tr>
                <th style={{ width: "50px" }}>No</th>
                <th>Recommendation</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {issues.length > 0 ? (
                issues.sort(compare).map((issue) => (
                  <tr>
                    <td>{issue.no}</td>
                    <td>{issue.title}</td>
                    <td>
                      <strong>{issue.status}</strong>
                      {issue.implementationText ? (
                        <>
                          <br />{issue.implementationText}
                        </>
                      ) : null}
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colspan="3" className="text-center">
                    <strong>Empty List</strong>
                  </td>
                </tr>
              )}
            </tbody>
          </Table>
        </>
      ) : null}
    </>
  );
}
