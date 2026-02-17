import React, { useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Snackbar } from "@material-ui/core";
import { Row, Col, Form } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";

import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import { useHistory } from "react-router-dom";
import { useForm } from "react-hook-form";
import { AddAlert } from "@material-ui/icons";
import { registerUser } from "../../services/oauth";
import {
  is_validate_email,
  is_validate_mobile,
  is_valid_TIN,
} from "../../utils/inputValidations";

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

export default function Register() {
  const history = useHistory();
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm();
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(false);
  const [forBusiness, setForBusiness] = React.useState(true);
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

  function resetInputs(inputData) {
    for (const [key, value] of Object.entries(inputData)) {
      setValue(key, "");
    }
  }

  function onSubmit(inputData) {
    if (inputData.confirmPassword !== inputData.password) {
      setErrorMsg("Password & Confirm Password are not similar!");
      showNotification("tr");
      return;
    }
    inputData = { ...inputData, forBusiness: forBusiness };
    registerUser(inputData)
      .then((data) => {
        if (data.data.success) {
          setErrorMsg("Registered Successfully!! Go to Login page to Login");
          resetInputs(inputData);
          //history.push("/login");
        } else if (data.data.errorMsg) {
          setErrorMsg(data.data.errorMsg);
        }
        showNotification("tr");
      })
      .catch((error) => {
        console.log(error);
        //setErrorMsg(error);
        showNotification("tr");
      });
  }

  return (
    <div className={classes.main}>
      <Grid container spacing={2} className="grid">
        <Grid item xs={3} md={3} className={classes.counter}></Grid>
        <Grid item xs={6} md={6} className={classes.counter}>
          <div class="row">
            <div class="bg">Create Account </div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)}>
            <Row className="g-2">
              <Col md>
                <Form.Label> </Form.Label>
                <input
                  type="checkbox"
                  checked={forBusiness}
                  onChange={(event) => setForBusiness(event.target.checked)}
                />{" "}
                For Business
                {!forBusiness ? (
                  <strong>
                    <br />
                    FOR INDIVIDUAL OR INTERNATIONAL USERS. ONLY TO BE USED TO
                    PROCURE ELECTRONIC MATERIALS.
                  </strong>
                ) : null}
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> User Name </Form.Label>
                <Form.Control
                  type="text"
                  {...register("userName", { required: true, maxLength: 50 })}
                />
              </Col>
              <Col md>
                <Form.Label> Email </Form.Label>
                <Form.Control
                  type="email"
                  {...register("email", {
                    validate: (value) => is_validate_email(value),
                  })}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> Full Name</Form.Label>
                <Form.Control
                  type="text"
                  {...register("fullName", { required: true, maxLength: 50 })}
                />
              </Col>
              {forBusiness ? (
                <Col md>
                  <Form.Label> Company Name</Form.Label>
                  <Form.Control
                    type="phone"
                    {...register("companyName", {
                      required: false,
                      maxLength: 255,
                    })}
                  />
                </Col>
              ) : null}
            </Row>
            {forBusiness ? (
              <>
                <Row className="g-2">
                  <Col md>
                    <Form.Label> TIN </Form.Label>
                    <Form.Control
                      type="text"
                      {...register("tin", {
                        validate: (value) => is_valid_TIN(value),
                      })}
                    />
                  </Col>
                  <Col md>
                    <Form.Label> Phone</Form.Label>
                    <Form.Control type="phone" {...register("phoneNo")} />
                  </Col>
                </Row>
                <Row className="g-2">
                  <Col md>
                    <Form.Label> Mobile No </Form.Label>
                    <Form.Control
                      type="text"
                      {...register("mobileNo", {
                        validate: (value) => is_validate_mobile(value),
                      })}
                    />
                  </Col>
                  <Col md>
                    <Form.Label> Address</Form.Label>
                    <Form.Control type="text" {...register("address")} />
                  </Col>
                </Row>
              </>
            ) : null}
            <Row className="g-2">
              <Col md>
                <Form.Label> Password</Form.Label>

                <Form.Control
                  type="password"
                  {...register("password", { required: true, maxLength: 200 })}
                />
              </Col>
              <Col md>
                <Form.Label> Confirm Password</Form.Label>

                <Form.Control
                  type="password"
                  {...register("confirmPassword", {
                    required: true,
                    maxLength: 200,
                  })}
                />
              </Col>
            </Row>
            <button type="submit" class="btn btn-success">
              Save
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
