import React from "react";
import { Card, Col, Row, Table } from "react-bootstrap";

export default function ApplicationDetailTbl({
  requirements,
  association,
  request,
  onPrintInvoice,
  onPrintReceipt,
  SubmitDoc,
  getLocal,
  getLocalByVal,
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
          ({getLocal("Invoice No")}:
          <a
            className="text-primary"
            style={{ cursor: "pointer" }}
            onClick={() => onPrintInvoice(req.id, req.invoiceNo)}
          >
            {req.invoiceNo}
          </a>
          ;
          {req.invoiceAmount
            ? ` ${getLocal("Amount")}: ${req.invoiceAmount};`
            : ""}
          {req.invoiceDateStr
            ? ` ${getLocal("Date")}: ${req.invoiceDateStr};`
            : ""}
          )
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
          {getLocal("Saved Document")}
        </a>
      );
    } else if (
      req.requirement.requirementTypeValue === 3 &&
      req.invoiceNo &&
      req.receiptNo
    ) {
      return req.printReceipt ? (
        <a
          className="text-primary"
          style={{ cursor: "pointer" }}
          onClick={() => onPrintReceipt(req.id, req.invoiceNo, req.receiptNo)}
        >
          {req.response}
        </a>
      ) : (
        req.response
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
          className="btn-sm btn btn-warning "
          style={{ float: "none", margin: "0" }}
          onClick={() => SubmitDoc(req)}
        >
          {getLocal("Submit")}
        </button>
      </div>
    ) : (
      ""
    );
  };
  return (
    <>
    <div id="showOnLargeScreen">
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>#</th>
            <th>{getLocal("Requirement")}</th>
            <th>{getLocal("Value")}</th>
            <th>{getLocal("Status")}</th>
            <th>{getLocal("Remark")}</th>
            <th>{getLocal("Actions")}</th>
          </tr>
        </thead>
        <tbody>
          {requirements.length > 0 ? (
            requirements
              // .filter((req) => req.isParent)
              .map((req, index) => (
                <tr key={index}>
                  <td>{req.requirement.orderNo}</td>
                  <td>{requirementColumn(req)}</td>
                  <td>{valueColumn(req)}</td>
                  <td>{req.reviewStatus}</td>
                  <td>{req.remark}</td>
                  <td>{actionColumn(req)}</td>
                </tr>
              ))
          ) : (
            <tr>
              <td colSpan={6} className="text-center">
                <strong>{getLocal("No Requirements to show")}</strong>
              </td>
            </tr>
          )}
        </tbody>
      </Table>
      </div>
      <div id="showOnSmallScreen">
        {requirements && requirements.length > 0 ? (
          requirements.map((req, i) => (
            <Card key={i} className="card__margin-bottom">
              <Card.Header>{requirementColumn(req)}</Card.Header>
              <Card.Body>
                <Row>
                  <Col>#</Col>
                  <Col>{req.requirement.orderNo}</Col>
                </Row>
                <Row>
                  <Col>{getLocal("Requirement")}</Col>
                  <Col>{requirementColumn(req)}</Col>
                </Row>
                <Row>
                  <Col>{getLocal("Value")}</Col>
                  <Col>{valueColumn(req)}</Col>
                </Row>
                <Row>
                  <Col>{getLocal("Status")}</Col>
                  <Col>{req.reviewStatus}</Col>
                </Row>
                <Row>
                  <Col>{getLocal("Remark")}</Col>
                  <Col>{req.remark}</Col>
                </Row>
                <Row>
                  <Col>{getLocal("Actions")}</Col>
                  <Col>{actionColumn(req)}</Col>
                </Row>
              </Card.Body>
            </Card>
          ))
        ) : (
          <Card  className="card__margin-bottom">
            <strong>{getLocal("No Requirements to show")}</strong>
          </Card>
        )}
      </div>
    </>
  );
}
