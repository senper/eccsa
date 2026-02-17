import React from "react";

import "./MainMenu.css";

import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";

class QuickLinks extends React.Component {
  render() {
    return (
      <div>
        <Card text="white" style={{ width: "18rem" }}>
          <Card.Header>Quick Links</Card.Header>
          <Card.Body>
            <Card.Text>
              <ul class="list-unstyled mb-0">
                <li>
                  {/* <a href="#!" class="text-green"> Organizational Structure</a> */}
                  <Link to="/about">About</Link>
                  {/* <link to="/eresources5" >xx</link> */}
                </li>
                <li>
                  <Link to="/assembly">Assembly</Link>
                </li>
                <li>
                  <Link to="/board" eventKey="board">
                    Board of Directors
                  </Link>
                </li>{" "}
                <li>
                  {" "}
                  <Link eventKey="team" to="/team" >
                    {" "}
                    The Secretariat
                  </Link>
                </li>{" "}
                <li>
                  {" "}
                  <Link eventKey="former" to="/former">
                    Former Presidents & Secretary G.
                  </Link>
                </li>{" "}
                <li>
                  {" "}
                  <Link eventKey="location" to="/location">
                    Our Location
                  </Link>
                </li>{" "}
                <li>
                  {" "}
                  <Link eventKey="partners" to="/partners">
                    Our Partners
                  </Link>
                </li>{" "}
                <li>
                  {" "}
                  <Link eventKey="splan" to="/splan">
                    Strategic Plan
                  </Link>
                </li>{" "}
                <li>
                  {" "}
                  <Link eventKey="chamberlaw" to="/chamberlaw">
                    Chamber Law (+Directives)
                  </Link>
                </li>{" "}
                <li>
                  {" "}
                  <Link eventKey="projects" to="/projects">
                    ECCSA Projects
                  </Link>
                </li>
              </ul>
            </Card.Text>
          </Card.Body>
        </Card>
      </div>
    );
  }
}

export default QuickLinks;
