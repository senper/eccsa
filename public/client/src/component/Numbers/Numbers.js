import { render } from "@testing-library/react";
import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Component } from "react";
import {
  Breadcrumb,
  Nav,
  Container,
  Carousel,
  Card,
  Row,
  Col,
  Form,
} from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBullhorn } from "@fortawesome/free-solid-svg-icons";
// import { faBook } from '@fortawesome/free-solid-svg-icons';
import { faBook } from "@fortawesome/free-solid-svg-icons";
import { faChalkboardTeacher } from "@fortawesome/free-solid-svg-icons";
import AnimatedNumber from "animated-number-react";
// import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "./Numbers.css";
// import 'font-awesome/css/font-awesome.min.css';
const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "15px",
    padding: "1px",
    row: "20px",
    background: "#fff",
  },
}));
class Numbers extends Component {
  state = {
    value: 1150,
    value2: 785,
    value3: 75,
    duration: 900,
  };
  formatValue = (value) => ` ${Number(value).toFixed(0)}`;
  render() {
    return (
      <div className="row">
        {/* <div className="row">
    <div className="bg">Our Statstics</div>
 </div>    */}
        <div className="row">
          <div className="col-xl-3 col-md-3 ">
            <div className="card  text-center border1 py-1">
              <div className="card-body">
                <div className="row no-gutters align-items-center">
                  <div className="col">
                    {/* <FontAwesomeIcon  /> */}
                    <div className="icon2">
                      <AnimatedNumber
                        value={this.state.value}
                        formatValue={this.formatValue}
                        duration={this.state.duration}
                        className="icon2"
                      />
                    </div>
                    <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                      <a href="#" className="nos">
                        Private Members
                      </a>
                    </div>
                  </div>
                  <div className="col-auto">
                    <i className="fas fa-calendar fa-2x text-gray-300"></i>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="col-xl-3 col-md-3  ">
            <div className="card text-center py-1 card-box  border2">
              <div className="card-body">
                <div className="row no-gutters align-items-center">
                  <div className="col">
                    {/* <FontAwesomeIcon  /> */}
                    <div className="icon2">
                      <AnimatedNumber
                        value={this.state.value2}
                        formatValue={this.formatValue}
                        duration={this.state.duration}
                        className="icon2"
                      />
                    </div>
                  </div>{" "}
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <a href="/capacity-building" className="nos">
                      Associations{" "}
                    </a>{" "}
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800"></div>
                  {/* </div> */}
                  <div className="col-auto"></div>
                </div>
              </div>
            </div>
          </div>

          <div className="col-xl-3 col-md-3 ">
            <div className="card text-center py-1 ard-box border3 ">
              <div className="card-body">
                <div className="row no-gutters align-items-center">
                  <div className="col">
                    {/* <FontAwesomeIcon  /> */}
                    <div className="icon2">
                      <AnimatedNumber
                        value={this.state.value}
                        formatValue={this.formatValue}
                        duration={this.state.duration}
                        className="icon2"
                      />{" "}
                    </div>
                  </div>
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <a href="/capacity-building" className="nos">
                      Processed Applications
                    </a>{" "}
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800"></div>
                </div>
                <div className="col-auto">
                  {/* <FontAwesomeIcon icon={["fab"]} /> */}
                  <i className="fas fa-calendar fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
          <div className="col-xl-3 col-md-3 ">
            <div className="card  text-center py-1 ard-box border4">
              <div className="card-body">
                <div className="row no-gutters align-items-center">
                  <div className="col">
                    {/* <FontAwesomeIcon  /> */}
                    <div className="icon2">
                      <AnimatedNumber
                        value={this.state.value3}
                        formatValue={this.formatValue}
                        duration={this.state.duration}
                        className="icon2"
                      />{" "}
                    </div>
                  </div>
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <a href="/capacity-building" className="nos">
                      Trainings
                    </a>{" "}
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800"></div>
                </div>
                <div className="col-auto">
                  {/* <FontAwesomeIcon icon={["fab"]} /> */}
                  <i className="fas fa-calendar fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Numbers;
