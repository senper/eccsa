import React from "react";
import { Row, Col, Form } from "react-bootstrap";
import { useForm } from "react-hook-form";
import {
  is_validate_email,
  is_validate_mobile,
  is_valid_TIN,
} from "../utils/inputValidations";
import { getEmailMobileVerification, registerUser } from "../api/oauth";
import { Grid, Snackbar } from "@mui/material";
import { AddAlert } from "@mui/icons-material";
import { useRouter } from "next/router";

export default function Register() {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm();
  const location = useRouter();
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(false);
  const [forBusiness, setForBusiness] = React.useState(true);
  const showNotification = (place, useTimeOut = true) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          if (useTimeOut) {
            setTimeout(function () {
              setError(false);
              setErrorMsg(null);
            }, 6000);
          }
        }
        break;
      default:
        break;
    }
  };

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
    var formData = new FormData();
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    registerUser(formData)
      .then((data) => {
        if (data.data.success) {
          setErrorMsg("Registered Successfully!!");
          showNotification("tr");
          const link = `${process.env.NEXT_PUBLIC_BASE_URL}/verifyAccount`;
          getEmailMobileVerification({Email: inputData.email, ConfirmationUrl: link}).then((sendData) => {
            if (sendData.data.success) {
              resetInputs(inputData);
              location.push("/verifyAccount/needEmailPhoneConfirmation");
            } else if (data.data.errorMsg) {
              setErrorMsg(data.data.errorMsg);
              showNotification("tr", false);
            } else {
              setErrorMsg("Failed to send verification code!");
              showNotification("tr", false);
            }
          });
        } else if (data.data.errorMsg) {
          var splited = data.data.errorMsg.split("\r\n");
          var htmlError = (
            <div>
              {splited.map((val, i) => (
                <span key={i}>
                  {val}
                  <br />
                </span>
              ))}
            </div>
          );
          setErrorMsg(htmlError);
          showNotification("tr", false);
        }
        showNotification("tr");
      })
      .catch((error) => {
        //console.log(error);
        //setErrorMsg(error);
        showNotification("tr");
      });
  }

  return (
    <div className="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={1} md={3}></Grid>
        <Grid item xs={11} md={6}>
          <div className="row">
            <div className="bg">Create Account </div>
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
                <Row className="g-2">
                  <Col md>
                    <Form.Label> Logo(PNG/Less Than 65Kb)</Form.Label>
                    <Form.Control
                      type="file"
                      accept="image/png"
                      {...register("logo")}
                    />
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
            <button type="submit" className="btn btn-success">
              Save
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item xs={1} md={3}></Grid>

        <br />
        <br />
      </Grid>
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : "Failed to Save Registration"}
        open={error}
        onClose={() => setError(false)}
      />
    </div>
  );
}
