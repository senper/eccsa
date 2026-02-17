import React from 'react';
import { Card, Col, Row, Table } from 'react-bootstrap';
import { excapeCharsReplacer } from '../../api/serviceRequest';

export default function ServiceRequestDetailTbl({
  requirements,
  serviceType,
  serviceRequest,
  RequirementReceptForm,
  onPrintInvoice,
  onPrintReceipt,
  SubmitDoc,
  getAttachment,
  getLocal,
  getLocalByVal,
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
              : settlment.receiptNo}{' '}
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
            {getLocal('Other Receipts')}:{' '}
            {settlments.map((set, index) => (
              <span key={index}>
                {set.extension ? getAttachment(set) : set.receiptNo}{' '}
              </span>
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
          ({getLocal('Invoice No')}:
          <a
            className="text-primary"
            style={{ cursor: 'pointer' }}
            onClick={() => onPrintInvoice(req.id, req.invoiceNo)}
          >
            {req.invoiceNo}
          </a>
          ;
          {req.invoiceAmount
            ? ` ${getLocal('Amount')}: ${req.invoiceAmount};`
            : ''}
          {req.invoiceDateStr
            ? ` ${getLocal('Date')}: ${req.invoiceDateStr};`
            : ''}
          )
        </>
      );
    }
    return (
      <>
        {req.requirement ? (
          <>
            {getLocalByVal(req.requirement.label, req.requirement.labelLocal)}{' '}
            {invoiceInfo}
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
      const fileName = req.id.substr(req.id.length - 15).replace('-', '');
      const relativePath = `${
        serviceType.serviceOwnerStr
          ? serviceType.serviceOwnerStr.toLowerCase()
          : ''
      }/documents/${fileName}${req.textResponse}`;
      return (
        <a
          href={`${process.env.NEXT_PUBLIC_API_URL}/${relativePath}`}
          target="_blank"
          rel="noreferrer"
        >
          {getLocal('Saved')} {getLocal('Document')}
        </a>
      );
    } else if (
      serviceType &&
      req.requirement.inputMethodValue === 11 &&
      req.tabularResponse
    ) {
      return <strong>{getLocal('Referer Below')}</strong>;
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
          {DisplayLastReceipt(req)}{' '}
          {req.paymentStatusInt == 3 || req.paymentStatusInt == 2
            ? '(' + req.paymentStatus + ')'
            : null}
          {RequirementReceptForm(req)}
          {req.printReceipt ? (
            <a
              className="text-primary"
              style={{ cursor: 'pointer' }}
              onClick={() =>
                onPrintReceipt(req.id, req.invoiceNo, req.receiptNo)
              }
            >
              ({getLocal('Download Receipt')})
            </a>
          ) : null}
          {DisplayOtherReceipts(req)} <br />
          {getLocal('Confirmed Amt')}: {req.amountPaid ? req.amountPaid : 0}
        </>
      );
    } else {
      return req.response;
    }
  };
  const actionColumn = (req) => {
    //console.log(req);
    return req.reviewStatusValue === 3 ||
      (req.requirement.isForOpponent &&
        req.requirement.requirementTypeValue == 1 &&
        req.reviewStatusValue === 0) ? (
      <div className="actions-right">
        <button
          type="button"
          className="btn-sm btn btn-warning "
          style={{ float: 'none', margin: '0' }}
          onClick={() => SubmitDoc(req)}
        >
          {' '}
          {getLocal('Submit')}
        </button>
      </div>
    ) : (
      ''
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
        rowData.forEach((d) =>
          inputDataStr.push(`"${d.text}": "${excapeCharsReplacer(d.value)}"`)
        );
        resultList.push({
          id: v,
          reqId: r.requirement.id,
          row: JSON.parse(`{${inputDataStr.toString()}}`),
        });
      });
    }
    const headersList = tabularSource(r);
    if (resultList.length > 0) {
      return resultList.map((s, index) => (
        <tr key={index}>
          {headersList.map((d, dindex) => (
            <td key={dindex}>{s.row[d.id]}</td>
          ))}
        </tr>
      ));
    } else {
      return (
        <tr>
          <td colSpan={headersList.length} className="text-center">
            <strong>{getLocal('No Data to show')}</strong>
          </td>
        </tr>
      );
    }
    //
  };
  const tabularResponsesCard = (r) => {
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
        rowData.forEach((d) =>
          inputDataStr.push(`"${d.text}": "${excapeCharsReplacer(d.value)}"`)
        );
        resultList.push({
          id: v,
          reqId: r.requirement.id,
          row: JSON.parse(`{${inputDataStr.toString()}}`),
        });
      });
    }
    const headersList = tabularSource(r);
    if (resultList.length > 0) {
      return resultList.map((s, index) => (
        <Card key={index}>
          <Card.Header>
            {headersList.length > 0 ? resultList[0].row[headersList[0].id] : ''}
          </Card.Header>
          <Card.Body>
            {headersList.map((d, index) => (
              <Row key={index}>
                <Col>{d.caption}</Col>
                <Col>{s.row[d.id]}</Col>
              </Row>
            ))}
          </Card.Body>
        </Card>
      ));
    } else {
      return (
        <Card>
          <Card.Body>
            <Row>
              <Col>
                <strong>{getLocal('No Data to show')}</strong>
              </Col>
            </Row>
          </Card.Body>
        </Card>
      );
    }
    //
  };

  return (
    <>
      <div id="showOnLargeScreen">
        <Table striped bordered hover size="sm" className="three-dtbl">
          <thead>
            <tr>
              <th>#</th>
              <th>{getLocal('Requirement')}</th>
              <th>{getLocal('Value')}</th>
              <th>{getLocal('Status')}</th>
              <th>{getLocal('Remark')}</th>
              <th>{getLocal('Actions')}</th>
            </tr>
          </thead>
          <tbody>
            {requirements.length > 0 ? (
              requirements
                //.filter((req) => req.requirement.inputMethodValue !== 11)
                .map((req, index) => (
                  <tr key={index}>
                    <td>
                      {/* {req && req.hasChild ? (
                    <a
                      data-tt="@q.OrderNo"
                      onclick="showHideAnswers(this);"
                      className="title"
                    >
                      {" "}
                      <i className="fas fa-angle-right"></i>
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
                  <strong>{getLocal('No Requirements to show')}</strong>
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
                  <Col>{getLocal('Requirement')}</Col>
                  <Col>{requirementColumn(req)}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Value')}</Col>
                  <Col>{valueColumn(req)}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Status')}</Col>
                  <Col>{req.reviewStatus}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Remark')}</Col>
                  <Col>{req.remark}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Actions')}</Col>
                  <Col>{actionColumn(req)}</Col>
                </Row>
              </Card.Body>
            </Card>
          ))
        ) : (
          <>No Requests Found</>
        )}
      </div>
      <div id="showOnLargeScreen">
        {requirements.length > 0
          ? requirements
              .filter((req) => req.requirement.inputMethodValue == 11)
              .map((req, index) => (
                <div key={index}>
                  <strong>
                    #{req.requirement.orderNo}{' '}
                    {getLocalByVal(
                      req.requirement.label,
                      req.requirement.labelLocal
                    )}
                  </strong>

                  <Table
                    striped
                    bordered
                    hover
                    size="sm"
                    className="three-dtbl"
                  >
                    <thead>
                      <tr>
                        {tabularSource(req).map((d, dindex) => (
                          <th key={dindex}>{d.caption}</th>
                        ))}
                      </tr>
                    </thead>
                    <tbody>{tabularResponses(req)}</tbody>
                  </Table>
                </div>
              ))
          : null}
      </div>
      <div id="showOnSmallScreen">
        {requirements.length > 0
          ? requirements
              .filter((req) => req.requirement.inputMethodValue == 11)
              .map((req, index) => (
                <div key={index}>
                  <strong>
                    #{req.requirement.orderNo}{' '}
                    {getLocalByVal(
                      req.requirement.label,
                      req.requirement.labelLocal
                    )}
                  </strong>
                  {tabularResponsesCard(req)}
                </div>
              ))
          : null}
      </div>
    </>
  );
}
