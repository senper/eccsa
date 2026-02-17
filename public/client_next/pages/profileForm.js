import React from "react";

import { Grid, Snackbar } from "@mui/material";
import { Col, Form, Row } from "react-bootstrap";
import { AddAlert } from "@mui/icons-material";
import {
  getEmailMobileVerification,
  logoff,
  updateProfile,
  userProfile,
} from "../api/oauth";
import {
  is_validate_email,
  is_valid_TIN,
  is_validate_mobile,
} from "../utils/inputValidations";
import { useForm } from "react-hook-form";
import { useRouter } from "next/router";
import { wrapper } from "../store";

export default function ProfileForm({ setUser, ...rest }) {
  window.scrollTo({
    top: 0,
  });
  const {
    register,
    handleSubmit,
    setValue,
    getValues,
    formState: { errors },
  } = useForm();
  const { store } = wrapper.useWrappedStore(rest);
  const location = useRouter();

  const [forBusiness, setForBusiness] = React.useState(true);
  const [userInfo, setUserInfo] = React.useState({});
  React.useEffect(() => {
    loadUserProfile();
  }, []);
  function loadUserProfile() {
    userProfile().then((data) => {
      setForBusiness(data.data.businessAccount);
      setUserInfo({ logo: data.data.logo });
      const values = getValues();
      for (const [key, value] of Object.entries(values)) {
        if (key == "logo") {
        } else {
          setValue(key, data.data[key]);
        }
      }
    });
  }
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
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

  function LogoImg() {
    if (userInfo.logo) {
      return (
        <img
          src={`${process.env.NEXT_PUBLIC_API_URL}/${userInfo.logo}`}
          alt="Logo"
          width={100}
          height={100}
        />
      );
    }
    return null;
  }

  function onSubmit(inputData) {
    var formData = new FormData();
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    updateProfile(formData)
      .then((data) => {
        if (data.data.success) {
          setErrorMsg("Profile Updated Successfully!");
          showNotification("tr");
          if (data.data.hasEmailOrMobileChange == true) {
            logoff(store);
            setUser(null);
            const link = `${process.env.NEXT_PUBLIC_BASE_URL}/verifyAccount`;
            getEmailMobileVerification({
              Email: inputData.email,
              ConfirmationUrl: link,
            }).then((sendData) => {
              if (sendData.data.success) {
              } else if (data.data.errorMsg) {
                setErrorMsg(data.data.errorMsg);
                showNotification("tr", false);
              } else {
                setErrorMsg("Failed to send verification code!");
                showNotification("tr", false);
              }
              location.push("/verifyAccount/needEmailPhoneConfirmation?c=1");
            });
          } else {
            loadUserProfile();
          }
          //history.push("/login");
        } else if (data.data.errorMsg) {
          setErrorMsg(data.data.errorMsg);
          showNotification("tr", false);
        }
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
            <div className="bg">Update Profile</div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)} encType="multipart/form-data">
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
                    readOnly
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
                      readOnly
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
                    <Form.Label>
                      {" "}
                      Change/Set Logo(PNG/Less Than 65Kb)
                    </Form.Label>
                    <Form.Control
                      type="file"
                      accept="image/png"
                      {...register("logo")}
                    />
                  </Col>
                  <Col md style={{ textAlign: "center" }}>
                    <Form.Label> Current Saved Logo</Form.Label> <br />
                    {LogoImg()}
                  </Col>
                </Row>
              </>
            ) : null}
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
        message={errorMsg ? errorMsg : "Failed to Save"}
        open={error}
        onClose={() => setError(false)}
      />
    </div>
  );
}

// export default App;
