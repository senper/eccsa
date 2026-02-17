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
import { changePassword } from "../../services/oauth";

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

export default function ChangePassword() {
  window.scrollTo({
    top: 0,
  });
  const history = useHistory();
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm();
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
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
    if (inputData.confirmPassword !== inputData.newPassword) {
      setErrorMsg("Password & Confirm Password are not similar!");
      showNotification("tr");
      return;
    }
    changePassword(inputData)
      .then((data) => {
        if (data.data.success) {
          setErrorMsg("Password changed successfully!");
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
            <div class="bg">Change Password </div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)}>
          <Row className="g-2">
              <Col md>
                <Form.Label> Old Password</Form.Label>

                <Form.Control
                  type="password"
                  {...register("oldPassword", { required: true, maxLength: 200 })}
                />
              </Col>
              <Col md>
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> New Password</Form.Label>

                <Form.Control
                  type="password"
                  {...register("newPassword", { required: true, maxLength: 200 })}
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
        message={errorMsg ? errorMsg : "Failed to Update"}
        open={error}
        closeNotification={() => setError(false)}
        close
      />
    </div>
  );
}

// export default App;
