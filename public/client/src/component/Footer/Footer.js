import { FaFacebookF, FaTwitter, FaTelegram, FaYoutube } from "react-icons/fa";
import React, { Component } from "react";
import ScrollToTop from "../ScrollToTop/ScrollToTop";
import "./Footer.css";
import { Col, Row } from "react-bootstrap";
import { Link } from "react-router-dom";
import Subscription from "./Subscription";
import store from "../../store";
import {
  clearSubscriptions,
  clearTopics,
} from "../../actions/subscriptionActions";
<link
  rel="stylesheet"
  href="https://fonts.googleapis.com/icon?family=Material+Icons"
/>;
const links = [
  { url: "https://esw.et/esw-trd/", label: "Electronic Single Window (eSW)" },
  { url: "https://customs.erca.gov.et/trade/", label: "Customs Trade Portal" },
  { url: "etrade.gov.et", label: "Online Trade Registration & License" },
  { url: "https://www.business.gov.et/business/home", label: "Ethiopian Business Portal" },
  { url: "https://investethiopia.gov.et/", label: "Ethiopian Investment Commission" },
  { url: "https://www.eservices.gov.et/", label: "Ethiopian e-Services Portal" },
];
export default function Footer() {
  return (
    <footer className="footer  text-center ">
      <div className="container p-4">
        <Row>
          {/* <Col sm={8}>
              <InputGroup className="mb-3">
                <InputGroup.Text
                  style={{ backgroundColor: "transparent", fontWeight: "bold", borderWidth: "0" }}
                >
                  Sign up for our newsletter
                </InputGroup.Text>
                <FormControl type="text" placeholder="Email address" />
                <Button
                  type="submit"
                  style={{ margin: "0", zIndex: "0" }}
                  variant="outline-secondary"
                  id="button-addon2"
                >
                  Subscribe
                </Button>
              </InputGroup>
            </Col> */}
          <Col>
            <section className="mb-4">
              <a
                className="btn btn-outline-light btn-floating m-1"
                href="https://twitter.com/EthiopiaChamber"
                role="button"
                target={"_blank"}
              >
                <i className="fab fa-twitter">
                  <FaTwitter />
                </i>
              </a>

              <a
                className="btn btn-outline-light btn-floating m-1"
                href="https://t.me/ethiopiachamber"
                role="button"
                target={"_blank"}
              >
                <i className="fab fa-google">
                  <FaTelegram />
                </i>
              </a>

              <a
                className="btn btn-outline-light btn-floating m-1"
                href="https://www.youtube.com/channel/UCMEZ4TO8dQiIJvk2IQkTxhA"
                role="button"
                target={"_blank"}
              >
                <i className="fab fa-instagram">
                  <FaYoutube />
                </i>
              </a>

              <a
                className="btn btn-outline-light btn-floating m-1"
                href="https://www.facebook.com/EthiopianChamber"
                role="button"
                target={"_blank"}
              >
                <i className="fab fa-linkedin-in">
                  <FaFacebookF />
                </i>
              </a>

              <ScrollToTop />
            </section>
          </Col>
        </Row>

        <section className="mb-4">
          <p></p>
        </section>

        <section className="">
          <div className="row">
            <Subscription />
            <div
              className="col-lg-3 col-md-3 mb-4 mb-md-0"
              style={{ textAlign: "left" }}
            >
              <h5 className="text-uppercase">Useful Links</h5>

              <ul className="footer-ul list-unstyled mb-0">
                {links.map((l) => (
                  <li>
                    <a target={"_blank"} href={l.url} className="link">
                      {l.label}
                    </a>
                  </li>
                ))}
              </ul>
            </div>

            <div
              className="col-lg-3 col-md-3 mb-4 mb-md-0"
              style={{ textAlign: "left" }}
            >
              <h5 className="text-uppercase">Important Links</h5>

              <ul className="footer-ul list-unstyled mb-0">
                <li>
                  <Link
                    className="link"
                    eventKey="Academy"
                    to={{
                      pathname: "/services",
                      state: {
                        serviceOwnerId: 8,
                        eventKey: "Academy",
                      },
                    }}
                  >
                    Academy
                  </Link>
                </li>
                <li>
                  <Link
                    className="link"
                    eventKey="Arbitration"
                    to={{
                      pathname: "/services",
                      state: {
                        serviceOwnerId: 16,
                        eventKey: "Arbitration",
                      },
                    }}
                  >
                    Arbitration
                  </Link>
                </li>
                <li>
                  <Link
                    className="link"
                    eventKey="Trade Facilitation"
                    to={{
                      pathname: "/services",
                      state: {
                        serviceOwnerId: 64,
                        eventKey: "Trade Facilitation",
                      },
                    }}
                  >
                    Trade Facilitation
                  </Link>
                </li>
                <li>
                  <Link
                    className="link"
                    eventKey="MatchMaking"
                    to={{
                      pathname: "/matchMaking",
                      state: {
                        eventKey: "MatchMaking",
                      },
                    }}
                  >
                    Match Making
                  </Link>
                </li>
                <li>
                  <Link
                    className="link"
                    eventKey="ppDialog"
                    to={{
                      pathname: "/ppDialog",
                      state: {
                        eventKey: "ppDialog",
                      },
                    }}
                  >
                    PPD
                  </Link>
                </li>
                <li>
                  <Link
                    className="link"
                    eventKey="issueForm"
                    to={{
                      pathname: "/issueForm",
                      state: {
                        eventKey: "issueForm",
                      },
                    }}
                  >
                    NTB Form
                  </Link>
                </li>
                <li>
                  <Link
                    className="link"
                    eventKey="faq"
                    to={{
                      pathname: "/faq",
                      state: {
                        eventKey: "faq",
                      },
                    }}
                  >
                    FAQ
                  </Link>
                </li>
              </ul>
            </div>

            <div
              className="col-lg-3  col-md-3 mb-4 mb-md-0"
              style={{ textAlign: "left" }}
            >
              <h5 className="text-uppercase">Contact Us</h5>

              <div className="address">
                <ul className="footer-ul list-unstyled mb-0">
                  <p>
                    <i className="fas fa-home me-3"> Mexico Square</i>
                  </p>
                  <p>
                    <i className="fas fa-home me-3">Addis Ababa, Ethiopia</i>
                  </p>
                  <p>
                    <i className="fas fa-phone me-3">
                      Phone: + 251 115 514 005
                    </i>
                  </p>
                  <p>
                    <i className="fas fa-print me-3">
                      Email:info@ethiopianchamber.com
                    </i>{" "}
                  </p>
                </ul>
              </div>
            </div>
          </div>
        </section>
      </div>

      <div className="text-center p-3">{/* © 2021 Copyright: */}</div>
    </footer>
  );
}
