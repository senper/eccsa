import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import "./ServiceRequestDetailTbl.css";
import { Link } from "react-router-dom";
import { Table } from "react-bootstrap";
import { excapeCharsReplacer } from "../../services/serviceRequest";

export default function ServiceRequestDetailTbl({
  requirements,
  serviceType,
  serviceRequest,
  RequirementReceptForm,
  onPrintInvoice,
  onPrintReceipt,
  SubmitDoc,
  getAttachment,
}) {
  function DisplayLastReceipt(res) {
    if (
      !res.invoice ||
      !res.invoice.settlments ||
      !res.invoice.settlments.$values
    ) {
      return null;
    }
    var settlment = res.invoice.settlments.$values.find(
      (s) => s.receiptNo == res.receiptNo
    );
    return (
      <>
        {settlment ? (
          <>
            {settlment.extension
              ? getAttachment(settlment)
              : settlment.receiptNo}{" "}
          </>
        ) : null}
      </>
    );
  }
  function DisplayOtherReceipts(res) {
    if (
      !res.invoice ||
      !res.invoice.settlments ||
      !res.invoice.settlments.$values
    ) {
      return null;
    }
    var settlments = res.invoice.settlments.$values.filter(
      (s) => s.receiptNo != res.receiptNo
    );
    return (
      <>
        {settlments.length > 0 ? (
          <>
            <br />
            Other Receipts:{" "}
            {settlments.map((set) => (
              <>{set.extension ? getAttachment(set) : set.receiptNo} </>
            ))}
          </>
        ) : null}
      </>
    );
  }

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
                serviceRequestId: serviceRequest.id,
                serviceTypeId: serviceRequest.serviceTypeId,
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
            {req.requirement.label} {invoiceInfo}
          </>
        ) : null}
      </>
    );
  };
  const valueColumn = (req) => {
    if (
      serviceType &&
      req.requirement.inputMethodValue === 1 &&
      req.textResponse
    ) {
      const fileName = req.id.substr(req.id.length - 15).replace("-", "");
      const relativePath = `${
        serviceType.serviceOwnerStr
          ? serviceType.serviceOwnerStr.toLowerCase()
          : ""
      }/documents/${fileName}${req.textResponse}`;
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
      serviceType &&
      req.requirement.inputMethodValue === 11 &&
      req.tabularResponse
    ) {
      return <strong>Referer Below</strong>;
    } else if (
      req.requirement.requirementTypeValue === 3 &&
      req.invoiceNo &&
      req.invoiceNo.toString().trim() &&
      (!req.receiptNo || req.reviewStatusValue === 3)
    ) {
      return RequirementReceptForm(req);
    } else if (
      req.requirement.requirementTypeValue === 3 &&
      req.invoiceNo &&
      req.receiptNo
    ) {
      return (
        <>
          {DisplayLastReceipt(req)}{" "}
          {req.paymentStatusInt == 3 || req.paymentStatusInt == 2
            ? "(" + req.paymentStatus + ")"
            : null}
          {RequirementReceptForm(req)}
          {req.printReceipt ? (
            <Link
              onClick={() =>
                onPrintReceipt(req.id, req.invoiceNo, req.receiptNo)
              }
              to={{
                state: {
                  serviceRequestId: serviceRequest.id,
                  serviceTypeId: serviceRequest.serviceTypeId,
                },
              }}
            >
             (Download Receipt)
            </Link>
          ) : null}
          {DisplayOtherReceipts(req)} <br />
          Confirmed Amt: {req.amountPaid ? req.amountPaid : 0}
        </>
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
  function compare(a, b) {
    if (a.no < b.no) {
      return -1;
    }
    if (a.no > b.no) {
      return 1;
    }
    return 0;
  }
  const tabularSource = (req) => {
    if (!serviceType || !serviceType.tabularRequirements) {
      return [];
    }
    var source = serviceType.tabularRequirements.$values.find(
      (l) => l.id == req.requirement.lookupTypeId
    );
    if (source && source.details && source.details.$values) {
      return source.details.$values.sort(compare);
    } else {
      return [];
    }
  };
  const tabularResponses = (r) => {
    //
    const resultList = [];
    if (r.tabularResponse && r.tabularResponse.$values) {
      let rowIds = r.tabularResponse.$values.map((t) => t.rowId);
      //get distinct rows
      rowIds = rowIds.filter((value, index, self) => {
        return self.indexOf(value) === index;
      });
      rowIds.forEach((v) => {
        const rowData = r.tabularResponse.$values.filter((t) => t.rowId == v);
        let inputDataStr = [];
        rowData.forEach((d) => inputDataStr.push(`"${d.text}": "${excapeCharsReplacer(d.value)}"`));
        resultList.push({
          id: v,
          reqId: r.requirement.id,
          row: JSON.parse(`{${inputDataStr.toString()}}`),
        });
      });
    }
    const headersList = tabularSource(r);
    if (resultList.length > 0) {
      return resultList.map((s) => (
        <tr>
          {headersList.map((d) => (
            <td>{s.row[d.id]}</td>
          ))}
        </tr>
      ));
    } else {
      return (
        <tr>
          <td colSpan={headersList.length} className="text-center">
            <strong>No Data to show</strong>
          </td>
        </tr>
      );
    }
    //
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
          {requirements.length > 0 ? (
            requirements
              //.filter((req) => req.requirement.inputMethodValue !== 11)
              .map((req) => (
                <tr>
                  <td>
                    {/* {req && req.hasChild ? (
                    <a
                      data-tt="@q.OrderNo"
                      onclick="showHideAnswers(this);"
                      class="title"
                    >
                      {" "}
                      <i class="fas fa-angle-right"></i>
                      &nbsp;req.requirement.orderNo
                    </a>
                  ) : (
                    req.requirement.orderNo
                  )} */}

                    {req.requirement.orderNo}
                  </td>
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
                <strong>No Requirements to show</strong>
              </td>
            </tr>
          )}
        </tbody>
      </Table>

      {requirements.length > 0
        ? requirements
            .filter((req) => req.requirement.inputMethodValue == 11)
            .map((req) => (
              <>
                <strong>
                  #{req.requirement.orderNo} {req.requirement.label}
                </strong>
                <Table striped bordered hover size="sm" className="three-dtbl">
                  <thead>
                    <tr>
                      {tabularSource(req).map((d) => (
                        <th>{d.caption}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody>{tabularResponses(req)}</tbody>
                </Table>
              </>
            ))
        : null}
    </>
  );
}
