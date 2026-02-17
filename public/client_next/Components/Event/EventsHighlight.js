import React from "react";

import { Carousel } from "react-bootstrap";

export default function EventsHighlight({
  events,
  setSelectedEvent,
  getLocal,
  getLocalByVal,
}) {
  return (
    <Carousel variant="light">
      {events.map((value, index) =>
        value.ownerStr ? (
          <Carousel.Item key={index}>
            <img
              style={{ height: "350px", opacity: "0.7" }}
              className="d-block w-100"
              src={`${
                process.env.NEXT_PUBLIC_API_URL
              }/${value.ownerStr.toLowerCase()}/documents/${value.id
                .substr(value.id.length - 15)
                .replace("-", "")}.jpg`}
              alt={`${getLocalByVal(value.title, value.titleLocal)} ${getLocal(
                "slide"
              )}`}
            />
            <Carousel.Caption>
              {/* <h3>{getLocalByVal(value.title, value.titleLocal)}</h3> 
              <div style={{ backgroundColor: "black", opacity: "0.5" }}>
                <p style={{ fontWeight: "bolder" }}>{value.description}</p>
              </div>*/}
              <button
                className="Primary"
                onClick={() => setSelectedEvent(value)}
              >
                {getLocal("Click to View Detail")}
              </button>
            </Carousel.Caption>
          </Carousel.Item>
        ) : null
      )}
    </Carousel>
  );
}
