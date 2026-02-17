import React, { useState } from "react";
import { Form } from "react-bootstrap";
import { login } from "../api/oauth";
import { useSelector } from "react-redux";
import { GoogleReCaptchaProvider } from "react-google-recaptcha-v3";
import { useRouter } from "next/router";
import { AddAlert } from "@mui/icons-material";
import { Grid, Snackbar } from "@mui/material";
import Link from "next/link";
import { wrapper } from "../store";
import { getLocalByKey } from "./api/menu";

export default function Login({ translations, query, ...rest }) {
  const { store } = wrapper.useWrappedStore(rest);
  const location = useRouter();
  const { locale } = location;
  const user = useSelector((state) => state.auth.user);
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [loggingIn, setLoggingIn] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState("");
  const [error, setError] = React.useState(false);
  let redirectValue = null;
  if (location.query) {
    const { redirect } = location.query;
    redirectValue = redirect;
  }
  function validateForm() {
    return userName.length > 0 && password.length > 0;
  }
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
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
    login({ userName: userName, password: password }, store)
      .then((result) => {
        if (result.data.success == true) {
          const redirect = redirectValue ? redirectValue : query ? query :  "/myServices";
          location.push(redirect);
          setLoggingIn(false);
        } else if (result.data.success == false) {           
          setErrorMsg(result.data.message);
          showNotification("tr");

          location.push("/verifyAccount/needEmailPhoneConfirmation" + (result.data.isNew ? "" : "?c=1"));
        } else {
          showNotification("tr");
          setLoggingIn(false);
        }
      })
      .catch((error) => {
        setLoggingIn(false);
        // console.log(error);
        showNotification("tr");
      });

    event.preventDefault();
  }
  return (
    <GoogleReCaptchaProvider reCaptchaKey="6Lf7kD0cAAAAAGx34iJzEpaOqxaJX-mOwusv_BmQ" 
    scriptProps={{
      async: false, // optional, default to false,
      defer: true, // optional, default to false
      appendTo: "body", // optional, default to "head", can be "head" or "body",
      nonce: "7331947154",
    }}>
      <div className="mmenu">
        <div
          style={{
            width: "100%",
            background: "#fff",
            padding: "10px",
          }}
        >
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}></Grid>

            <Grid item xs={1} sm={3} md={3}></Grid>
            <Grid item xs={12} sm={8} md={6}>
              <div className="row">
                <div className="bg">{getLocal("Login")} </div>
              </div>
              <form onSubmit={handleSubmit}>
                <Form.Group size="lg" controlId="userName">
                  <Form.Label>{getLocal("User Name")}</Form.Label>
                  <Form.Control
                    autoFocus
                    type="userName"
                    value={userName}
                    autoComplete="off"
                    required
                    onChange={(e) => setUserName(e.target.value)}
                  />
                </Form.Group>
                <Form.Group size="lg" controlId="password">
                  <Form.Label>{getLocal("Password")}</Form.Label>
                  <Form.Control
                    type="password"
                    value={password}
                    required
                    autoComplete="off"
                    onChange={(e) => setPassword(e.target.value)}
                  />
                </Form.Group>
                <button
                  type="submit"
                  className="btn btn-outline-success"
                  disabled={loggingIn}
                >
                  {loggingIn
                    ? `${getLocal("Logging-in")} ....`
                    : getLocal("Login")}
                </button>
              </form>
              {/* <a href="/myServices" className="btn btn-success">Login</a> */}
              <br />
              {getLocal("Don't have an account")}?{" "}
              <Link
                href="/register"
                className="btn btn-primary"
                style={{ float: "none", margin: "0" }}>

                {getLocal("Register")}

              </Link>
              <br />
              <Link
                href="/resetPassword/passwordReset"
                style={{ float: "none", margin: "0" }}>

                {getLocal("Forget Password?")}

              </Link>
            </Grid>
            <Grid item xs={1} md={3}></Grid>

            <br />
            <br />
            <Snackbar
              place="tr"
              color="danger"
              icon={AddAlert}
              message="Failed to authenticate"
              open={error}
              onClose={() => setError(false)}
            />
          </Grid>
        </div>
      </div>
    </GoogleReCaptchaProvider>
  );
}
