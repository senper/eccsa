import { FaFacebookF, FaTwitter, FaTelegram, FaYoutube } from "react-icons/fa";
import React from "react";
import ScrollToTop from "../ScrollToTop/ScrollToTop";
import { Col, Row } from "react-bootstrap";
import Subscription from "./Subscription";
import Link from "next/link";
import { getFooterMenu } from "../../pages/api/menu";

{
  /* <link
  rel="stylesheet"
  href="https://fonts.googleapis.com/icon?family=Material+Icons"
/>; */
}

<link
  rel="stylesheet"
  href={process.env.NEXT_PUBLIC_BASE_URL + "/styles/material-icons.css"}
/>;

export default function Footer({ getLocal, getLocalByVal }) {
  const [links, setLinks] = React.useState([]);
  const [footerMenu, setFooterMenu] = React.useState([]);
  const [contactUs, setContactUs] = React.useState(null);
  React.useEffect(() => {
    getFooterMenu().then((data) => {
      setFooterMenu(data.data.importantLinks.$values);
      setLinks(data.data.usefulLinks.$values);
      setContactUs(data.data.contactUs);
    });
  }, []);
  const MenuItem = ({ item }) => {
    return (
      <li>
        <Link href={item.path} passHref className="link">
          {getLocalByVal(item.title, item.titleLocal)}
        </Link>
      </li>
    );
  };
  return (
    <>
      <footer className="footer  text-center ">
        <div className="container p-4">
          <Row>
            <Col>
              <section className="mb-4">
                <a
                  className="btn btn-outline-light btn-floating m-1"
                  href="https://twitter.com/EthiopiaChamber"
                  role="button"
                  target={"_blank"}
                  rel="noreferrer"
                  aria-label="Ethiopia Chamber twitter"
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
                  rel="noreferrer"
                  aria-label="Ethiopia Chamber telegram"
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
                  rel="noreferrer"
                  aria-label="Ethiopia Chamber Instagram"
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
                  rel="noreferrer"
                  aria-label="Ethiopia Chamber Facebook"
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

          <section>
            <div className="row">
              <Subscription />
              <div
                className="col-lg-3 col-md-3 mb-4 mb-md-0"
                style={{ textAlign: "left" }}
              >
                <h5 className="text-uppercase">{getLocal("Useful Links")}</h5>

                <ul className="footer-ul list-unstyled mb-0">
                  {links
                    ? links.map((l, index) => (
                        <li key={index}>
                          <a
                            target={"_blank"}
                            href={l.path}
                            className="link"
                            rel="noreferrer"
                          >
                            {getLocalByVal(l.title, l.titleLocal)}
                          </a>
                        </li>
                      ))
                    : null}
                </ul>
              </div>

              <div
                className="col-lg-3 col-md-3 mb-4 mb-md-0"
                style={{ textAlign: "left" }}
              >
                <h5 className="text-uppercase">
                  {getLocal("Important Links")}
                </h5>

                <ul className="footer-ul list-unstyled mb-0">
                  {footerMenu
                    ? footerMenu.map((menu, index) => (
                        <MenuItem item={menu} key={index} />
                      ))
                    : null}
                </ul>
              </div>

              <div
                className="col-lg-3  col-md-3 mb-4 mb-md-0"
                style={{ textAlign: "left" }}
              >
                <h5 className="text-uppercase">{getLocal("Contact Us")}</h5>
                {contactUs ? (
                  <div className="address">
                    <div className="footer-ul list-unstyled mb-0">
                      <p>
                        <i className="fas fa-home me-3">
                          {getLocalByVal(
                            contactUs.address,
                            contactUs.addressLocal
                          )}{" "}
                        </i>
                      </p>
                      <p>
                        <i className="fas fa-home me-3">
                          {getLocalByVal(
                            contactUs.location,
                            contactUs.locationLocal
                          )}
                        </i>
                      </p>
                      <p>
                        <i className="fas fa-phone me-3">
                          {getLocal("Phone")}: {contactUs.phoneNo}
                        </i>
                      </p>
                      <p>
                        <i className="fas fa-print me-3">
                          {getLocal("Email")}:{contactUs.email}
                        </i>{" "}
                      </p>
                    </div>
                  </div>
                ) : null}
              </div>
            </div>
          </section>
        </div>
      </footer>
    </>
  );
}
