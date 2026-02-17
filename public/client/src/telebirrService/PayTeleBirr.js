import React, { useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Snackbar } from "@material-ui/core";
import { Row, Col, Form } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";

import "bootstrap/dist/css/bootstrap.css";
import Footer from "../component/Footer/Footer";
import { useHistory } from "react-router-dom";
import { AddAlert } from "@material-ui/icons";
import { sendNotify } from "../services/telepay";
import { useParams } from "react-router-dom";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "15px",
    padding: "1px",
    row: "20px",
    background: "#fff",
  },
  main: {
    width: "100%",
    background: "#fff",
    padding: "10px",
  },
  paper: {
    // padding:theme.spacing(1),
    textAlign: "left",
    color: "#198754",
    background: "#aaf",
    padding: "10px",
  },
  appBar: {
    color: "#000",
    background: "#fff",
  },
  margin: {
    marginLeft: "100px",
    marginTop: "5px",
    color: "primary",
  },
}));

export default function PayTeleBirr() {
  const history = useHistory();
  let params = useParams();
  const [invoiceNo, setInvoiceNo] = React.useState("111111");
  const [amount, setAmount] = React.useState(100);
  const [userNumber, setUserNumber] = React.useState("");
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState("");
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  const classes = useStyle();

  function onSubmit(e) {
    e.preventDefault();
    if (!userNumber) {
      setErrorMsg("User Number Required!");
      showNotification("tr");
      return;
    }

    sendNotify("xxx", "ttt")
      .then((res) => {
        if (params.returnUrl) {
          window.open(`${process.env.REACT_APP_BASE_URL}/serviceRequestDetailParam/a94290f1-99c6-46dc-bb0b-e117bba53659/82baa313-a2db-43a6-8e4a-56455ae00416`, "_self");
        }
      })
      .catch((error) => {
        console.error(error);
        return error;
      });
  }

  return (
    <div className={classes.main}>
      <Grid container spacing={2} className="grid">
        <Grid item xs={3} md={3} className={classes.counter}></Grid>
        <Grid item xs={6} md={6} className={classes.counter}>
          <div class="row">
            <div class="bg">Pay With Tele Birr </div>
          </div>
          <Form onSubmit={onSubmit}>
            <Row className="g-2">
              <Col md={6}>
                <Form.Label> User Number</Form.Label>
                <Form.Control
                  type="text"
                  value={userNumber}
                  onChange={(evt) => setUserNumber(evt.target.value)}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md={6}>
                <Form.Label> Invoice No</Form.Label>
                <Form.Control type="text" readOnly value={invoiceNo} />
              </Col>
              <Col md={6}>
                <Form.Label> Amount</Form.Label>
                <Form.Control type="text" readOnly value={amount} />
              </Col>
            </Row>
            <button type="submit" class="btn btn-success">
              Accept Payment
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item xs={3} md={3} className={classes.counter}></Grid>

        <br />
        <br />
        
      </Grid>
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : "Failed to Save Registration"}
        open={error}
        closeNotification={() => setError(false)}
        close
      />
    </div>
  );
}

// export default App;
