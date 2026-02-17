import { Card, Col, Form, InputGroup, Row } from 'react-bootstrap';
import React from 'react';
import { Table } from 'react-bootstrap';

export default function ServiceRequestFollowupTbl({
  followups,
  serviceType,
  getLocal,
  getLocalByVal,
}) {
  const getFollowupDocument = (followup) => {
    if (
      !followup.extension ||
      followup.extension.trim() == '' ||
      !serviceType.serviceOwner
    )
      return '-';
    const fileName = followup.id
      .substr(followup.id.length - 15)
      .replace('-', '');
    const relativePath = `${serviceType.serviceOwner.toLowerCase()}/documents/${fileName}${
      followup.extension
    }`;

    return (
      <a
        href={`${process.env.NEXT_PUBLIC_API_URL}/${relativePath}`}
        target="_blank"
        rel="noreferrer"
      >
        {getLocal('Document')}
      </a>
    );
  };
  return (
    <>
      <div id="showOnLargeScreen">
        <Table striped bordered hover size="sm" className="three-dtbl">
          <thead>
            <tr>
              <th>{getLocal('Title')}</th>
              <th>{getLocal('Date')}</th>
              <th>{getLocal('Time')}</th>
              <th>{getLocal('Description')}</th>
              <th>{getLocal('Document')}</th>
              <th>{getLocal('Status')}</th>
            </tr>
          </thead>
          <tbody>
            {followups.length > 0 ? (
              followups.map((fup, index) => (
                <tr key={index}>
                  <td>{fup.title}</td>
                  <td>{fup.startDateStr}</td>
                  <td>{fup.startTimeStr}</td>
                  <td>{fup.description}</td>
                  <td>{getFollowupDocument(fup)}</td>
                  <td>{fup.status}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={6} className="text-center">
                  <strong>{getLocal('No Followup to show')}</strong>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      </div>
      <div id="showOnSmallScreen">
        {followups.length > 0 ? (
          followups.map((fup, index) => (
            <Card key={index} className="card__margin-bottom">
              <Card.Header>
                {fup.title} - {fup.startDateStr}
              </Card.Header>
              <Card.Body>
                <Row>
                  <Col>{getLocal('Title')}</Col>
                  <Col>{fup.title}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Date')}</Col>
                  <Col>{fup.startDateStr}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Time')}</Col>
                  <Col>{fup.startTimeStr}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Description')}</Col>
                  <Col>{fup.description}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Document')}</Col>
                  <Col>{getFollowupDocument(fup)}</Col>
                </Row>
                <Row>
                  <Col>{getLocal('Status')}</Col>
                  <Col>{fup.status}</Col>
                </Row>
              </Card.Body>
            </Card>
          ))
        ) : (
          <Card>
            <Card.Body>
              <strong>{getLocal('No Followup to show')}</strong>
            </Card.Body>
          </Card>
        )}
      </div>
    </>
  );
}
