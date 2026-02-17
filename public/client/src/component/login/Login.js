import React, { useState } from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper, Button, Snackbar } from "@material-ui/core";
import AppBar from "@material-ui/core/AppBar";
import { Form } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import { Link } from "react-router-dom";
import "../../App.css";

import Footer from "../Footer/Footer";
import { login } from "../../services/oauth";
import { AddAlert } from "@material-ui/icons";
import { useHistory } from "react-router-dom";
import { useSelector } from "react-redux";
import { GoogleReCaptchaProvider } from "react-google-recaptcha-v3";
import { useLocation } from "react-router-dom";

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

    // elevation:'0',
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

export default function Login() {
  const location = useLocation();
  const user = useSelector((state) => state.auth.user);
  const history = useHistory();
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [loggingIn, setLoggingIn] = React.useState(false);
  const [error, setError] = React.useState(false);
  const classes = useStyle();
  let redirectValue = null;
  if (location.state) {
    const { redirect } = location.state;
    redirectValue = redirect;
    console.log(location.state);
  }
  function validateForm() {
    return userName.length > 0 && password.length > 0;
  }

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

  function handleSubmit(event) {
    setLoggingIn(true);
    login({ userName: userName, password: password })
      .then((token) => {
        setLoggingIn(false);
        if (token) {
          const redirect = redirectValue ? redirectValue : "/myServices";
          history.push({
            pathname: redirect,
            state: {
              eventKey: redirect.replaceAll('/', ""),
              navTo: "",
              ...location.state
            },
          });
        } else {
          showNotification("tr");
        }
      })
      .catch((error) => {
        setLoggingIn(false);
        console.log(error);
        showNotification("tr");
      });

    event.preventDefault();
  }
  return (
    <GoogleReCaptchaProvider reCaptchaKey="6Lf7kD0cAAAAAGx34iJzEpaOqxaJX-mOwusv_BmQ">
      <div class="mmenu">
        <div className={classes.main}>
          <Grid container spacing={2} className="grid">
            <Grid item xs={12} md={12}></Grid>

            <Grid item xs={3} md={3} className={classes.counter}></Grid>
            <Grid item xs={6} md={6} className={classes.counter}>
              <div class="row">
                <div class="bg">Login </div>
              </div>
              <form onSubmit={handleSubmit}>
                <Form.Group size="lg" controlId="userName">
                  <Form.Label>User Name</Form.Label>
                  <Form.Control
                    autoFocus
                    type="userName"
                    value={userName}
                    required
                    onChange={(e) => setUserName(e.target.value)}
                  />
                </Form.Group>
                <Form.Group size="lg" controlId="password">
                  <Form.Label>Password</Form.Label>
                  <Form.Control
                    type="password"
                    value={password}
                    required
                    onChange={(e) => setPassword(e.target.value)}
                  />
                </Form.Group>
                <button
                  type="submit"
                  className="btn btn-outline-success"
                  disabled={loggingIn}
                >
                  {loggingIn ? "Logging-in ...." : "Login"}
                </button>
              </form>
              {/* <a href="/myServices" class="btn btn-success">Login</a> */}
              <br />
              Don't have an account?{" "}
              <Link
                to="/register"
                className="btn btn-primary"
                style={{ float: "none", margin: "0" }}
              >
                Register
              </Link>
            </Grid>
            <Grid item xs={3} md={3} className={classes.counter}></Grid>

            <br />
            <br />
            <Snackbar
              place="tr"
              color="danger"
              icon={AddAlert}
              message="Failed to authenticate"
              open={error}
              closeNotification={() => setError(false)}
              close
            />
          </Grid>
        </div>
      </div>
    </GoogleReCaptchaProvider>
  );
}
