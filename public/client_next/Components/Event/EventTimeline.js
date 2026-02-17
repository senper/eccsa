import Link from "next/link";
import React from "react";
import { Card, Col, Row } from "react-bootstrap";

function EventTimeline({ selectedEvent, getLocal, getLocalByVal }) {
  var token = localStorage.getItem("access_token");
  const getTitle = (detail) => {
    const value = selectedEvent.details.$values.find(
      (d) => d.$id == detail.$ref
    );
    let color = "text-danger";
    if (value.isFuture) {
      color = "text-primary";
    } else if (!value.isEnded) {
      color = "text-success";
    }
    let formUrl = `/myServices/serviceRequestForm?serviceTypeId=${value.serviceTypeId}`;
    if (!token) {
      formUrl = `/login?redirect=${formUrl}`;
    }
    const title =
      value.serviceTypeId && !value.isEnded && !value.isFuture ? (
        (<Link href={formUrl} className={color}>

          {getLocalByVal(value.title, value.titleLocal)}

        </Link>)
      ) : (
        getLocalByVal(value.title, value.titleLocal)
      );

    return { value, title, color };
  };
  return (
    <div>
      <div id="showOnLargeScreen">
        <table className="table  table-striped">
          <thead>
            <tr>
              <th>{getLocal("Activity")}</th>
              <th>{getLocal("Start Date")}</th>
              <th>{getLocal("End Date")}</th>
            </tr>
          </thead>
          <tbody>
            {selectedEvent.detailsSorted.$values.map((detail, index) => {
              const { value, title, color } = getTitle(detail);
              return (
                <tr key={index}>
                  <td className={color}>{title}</td>
                  <td className={color}>{value.startDateStr}</td>
                  <td className={color}>{value.endDateStr}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
      <div id="showOnSmallScreen">
        {selectedEvent.detailsSorted.$values &&
        selectedEvent.detailsSorted.$values.length > 0 ? (
          selectedEvent.detailsSorted.$values.map((detail, i) => {
            const { value, title, color } = getTitle(detail);
            return (
              <Card key={i}>
                <Card.Header style={{ color: "white" }}>{title}</Card.Header>
                <Card.Body>
                  <Row className={color}>
                    <Col>{getLocal("Activity")}</Col>
                    <Col>{title}</Col>
                  </Row>
                  <Row className={color}>
                    <Col>{getLocal("Start Date")}</Col>
                    <Col>{value.startDateStr}</Col>
                  </Row>
                  <Row className={color}>
                    <Col>{getLocal("End Date")}</Col>
                    <Col>{value.endDateStr}</Col>
                  </Row>
                </Card.Body>
              </Card>
            );
          })
        ) : (
          <>No Activity</>
        )}
      </div>
    </div>
  );
}

export default EventTimeline;
