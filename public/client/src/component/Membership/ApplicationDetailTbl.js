import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import "./ApplicationDetailTbl.css";
import { Link } from "react-router-dom";
import { Table } from "react-bootstrap";

export default function ApplicationDetailTbl({
  requirements,
  association,
  request,
  onPrintInvoice,
  onPrintReceipt,
  SubmitDoc,
}) {
  const requirementColumn = (req) => {
    let invoiceInfo = <></>;
    if (
      req.requirement.requirementTypeValue === 3 &&
      req.invoiceNo &&
      req.invoiceNo.toString().trim()
    ) {
      invoiceInfo = (
        <>
          (Invoice No:
          <Link
            onClick={() => onPrintInvoice(req.id, req.invoiceNo)}
            to={{
              state: {
                serviceRequestId: request.id,
                serviceTypeId: request.serviceTypeId,
              },
            }}
          >
            {req.invoiceNo}
          </Link>
          ;{req.invoiceAmount ? ` Amount: ${req.invoiceAmount};` : ""}
          {req.invoiceDateStr ? ` Date: ${req.invoiceDateStr};` : ""})
        </>
      );
    }
    return (
      <>
        {req.requirement ? (
          <>
            {req.requirement.name} {invoiceInfo}
          </>
        ) : null}
      </>
    );
  };
  const valueColumn = (req) => {
    if (
      association &&
      req.requirement.inputMethodValue === 1 &&
      req.textResponse
    ) {
      const fileName = req.id.substr(req.id.length - 15).replace("-", "");
      const relativePath = `membership/documents/${fileName}${req.textResponse}`;
      return (
        <a
          href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
          target="_blank"
          rel="noreferrer"
        >
          Saved Document
        </a>
      );
    } else if (
      req.requirement.requirementTypeValue === 3 &&
      req.invoiceNo &&
      req.receiptNo
    ) {
      return (
        req.printReceipt ? <Link
          onClick={() => onPrintReceipt(req.id, req.invoiceNo, req.receiptNo)}
          to={{
            state: {
              serviceRequestId: request.id,
              serviceTypeId: request.serviceTypeId,
            },
          }}
        >
          {req.response}
        </Link> : req.response 
      );
    } else {
      return req.response;
    }
  };
  const actionColumn = (req) => {
    return req.reviewStatusValue === 3 ? (
      <div className="actions-right">
        <button
          type="button"
          class="btn-sm btn btn-warning "
          style={{ float: "none", margin: "0" }}
          onClick={() => SubmitDoc(req)}
        >
          {" "}
          Submit
        </button>
      </div>
    ) : (
      ""
    );
  };
  return (
    <>
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>#</th>
            <th>Requirement</th>
            <th>Value</th>
            <th>Status</th>
            <th>Remark</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {requirements.length > 0 ? requirements
           // .filter((req) => req.isParent)
            .map((req) => (
              <tr>
                <td>
                  {req.requirement.orderNo}
                </td>
                <td>{requirementColumn(req)}</td>
                <td>{valueColumn(req)}</td>
                <td>{req.reviewStatus}</td>
                <td>{req.remark}</td>
                <td>{actionColumn(req)}</td>
              </tr>
            )) : <tr><td colSpan={6} className="text-center"><strong>No Requirements to show</strong></td></tr>}
        </tbody>
      </Table>
    </>
  );
}
