// import { Icon } from '@material-ui/core/HomeIcon';
import ThreeDRotation from "@material-ui/icons/ThreeDRotation";
import BookmarkOutlinedIcon from "@material-ui/icons/BookmarkOutlined";
import React, { Component } from "react";
// import { Breadcrumb,Nav, Container, Carousel, Card, Row, Col, Form } from "react-bootstrap";
// import './Footer.css'
<link
  rel="stylesheet"
  href="https://fonts.googleapis.com/icon?family=Material+Icons"
/>;
export default function Partners() {
  const partners = [
    "client-1.png",
    "client-2.jpg",
    "client-3.png",
    "client-4.jpg",
    "Irish Aid Logo.jpg",
  ];
  return (
    <footer class="text-center text-white">
      <div class="container p-4">
        <section class="">
          <div class="row">
            {partners.map((v) => (
              <div class="col-lg-2 col-md-3">
                <div
                  class="bg-image hover-overlay ripple shadow-1-strong rounded"
                  data-ripple-color="light"
                >
                  <img
                    style={{ height: "80px", maxWidth: "150px" }}
                    src={"./images/clients/" + v}
                    className="w-100"
                  />
                  <a href="#!">
                    <div class="mask"></div>
                  </a>
                </div>
              </div>
            ))}
          </div>
        </section>
      </div>
    </footer>
  );
}
