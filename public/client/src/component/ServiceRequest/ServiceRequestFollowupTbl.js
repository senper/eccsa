import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import "./ServiceRequestDetailTbl.css";
import { Table } from "react-bootstrap";

export default function ServiceRequestFollowupTbl({
  followups, serviceType
}) {
  const getFollowupDocument = (followup) => {
    if(!followup.extension || followup.extension.trim() == "" ||
    !serviceType.serviceOwner) return "-";
    const fileName = followup.id
        .substr(followup.id.length - 15)
        .replace("-", "");
      const relativePath = `${serviceType.serviceOwner.toLowerCase()}/documents/${fileName}${
        followup.extension}`;

      return (
        <a
            href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            Document
          </a>
      )
  }
  return (
    <>
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>Title</th>
            <th>Date</th>
            <th>Time</th>
            <th>Description</th>
            <th>Document</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {followups.length > 0 ? followups
            .map((fup) => (
              <tr>               
                <td>{
                fup.title
                }</td>
               <td>{fup.startDateStr}</td>
                <td>{fup.startTimeStr}</td>
                <td>{fup.description}</td>                
                <td>{getFollowupDocument(fup)}</td>
                <td>{fup.status}</td>
              </tr>
            )): <tr><td colSpan={6} className="text-center"><strong>No Followup to show</strong></td></tr>}
        </tbody>
      </Table>
    </>
  );
}
