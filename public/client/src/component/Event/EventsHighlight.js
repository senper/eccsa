// import { Component } from 'react';
import React from "react";

import {
  Carousel,
} from "react-bootstrap";

import "./EventsHighlight.css";

function EventsHighlight(props) {
  return (
    <Carousel variant="light">
      {props.events.map((value) => (
       value.ownerStr ? (<Carousel.Item>
        <img
          style={{ height: "350px", opacity: "0.7" }}
          className="d-block w-100"
          src={`${process.env.REACT_APP_API_URL}/${value.ownerStr.toLowerCase()}/documents/${value.id.substr(value.id.length - 15).replace("-", "")}.jpg`}
          alt={`${value.title} slide`}
        />
        <Carousel.Caption>
          <h3>{value.title}</h3>
          <div style={{ backgroundColor: "black", opacity: "0.5" }}>
          <p style={{fontWeight: "bolder"}}>{value.description}</p>
          </div>
          <button className="Primary" onClick={() => props.setSelectedEvent(value)}>Click to View Detail</button>
        </Carousel.Caption>
      </Carousel.Item>) : null
      ))}
    </Carousel>
  );
}

export default EventsHighlight;
