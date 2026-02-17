import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";
import { serviceTypes } from "variables/lookups.js";

import { cardTitle } from "assets/jss/material-dashboard-pro-react.js";

import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import TextareaAutosize from "@material-ui/core/TextareaAutosize";
import MenuItem from "@material-ui/core/MenuItem";
import { Info } from "@material-ui/icons";
import { Link } from "react-router-dom";
import { FormLabel } from "@material-ui/core";
import CustomInput from "components/vender/CustomInput/CustomInput";
import Button from "components/vender/CustomButtons/Button.js";
import { useForm, Controller } from "react-hook-form";

import styles from "assets/jss/material-dashboard-pro-react/views/regularFormsStyle";

const useStyles = makeStyles(styles);

const statusList = [
  {
    id: 1,
    name: "Not Applicable",
  },
  {
    id: 2,
    name: "Approve",
  },
  {
    id: 3,
    name: "Reject",
  },
];

const requestStatusList = [
  {
    id: 1,
    name: "Resubmit",
  },
  {
    id: 2,
    name: "Approve",
  },
  {
    id: 3,
    name: "Reject",
  },
];

const inputDocuments = [
  {
    id: 1,
    name: "Payment Invoice",
    fileUrl: "/assets/paymentInvoicetest.jpg",
    status: 0,
  },
  {
    id: 2,
    name:
      "Copy of the certificate (Signed by relevant government organizations)",
    fileUrl: "/assets/paymentInvoicetest.jpg",
    status: 0,
  },
];

// eslint-disable-next-line react/prop-types
const RejectRemark = ({ id }) => {
  return (
    <>
      <label>Remark</label>
      <textarea rows={3} placeholder="Reject Remark"></textarea>
    </>
  );
};

export default function TradeFacilitationDetail() {
  const [documents, setDocuments] = React.useState(inputDocuments);
  const [showRequestStatus, setShowRequestStatus] = React.useState(false);
  const [anyDocRejected, setAnyDocRejected] = React.useState(false);
  const [rejectedCount, setRejectedCount] = React.useState(0);
  const { register: docStatusForm, handleSubmit, control } = useForm({
    mode: "onChange",
  });
  React.useEffect(() => {
    setDocuments(inputDocuments);
    setRejectedCount(documents.filter((value) => value.status == 3).length);
  }, []);

  const classes = useStyles();
  const handleStatusChange = (event, docId) => {
    const docs = documents.filter((value) => value.id == docId);
    if (docs.length >= 1) {
      docs[0].status = event.target.value;
    }

    setShowRequestStatus(
      documents.filter((value) => value.status === 0).length <= 0
    );
    setAnyDocRejected(
      documents.filter((value) => value.status == 3).length > 0
    );
    setRejectedCount(documents.filter((value) => value.status == 3).length);
  };
  const handleRequestStatusChange = (event) => {
    console.log(event.target.value);
  };
  const onSubmitLogin = (data) => {
    console.log(data);
  };
  return (
    <GridContainer>
      <GridItem xs={12}>
        <Card>
          <CardHeader color="primary" icon>
            <CardIcon color="primary">{"Service Request Info."}</CardIcon>
            <Link
              style={{ color: "orange" }}
              to="/tradePromotion/trade_facilitation"
            >
              {"<< back to list"}
            </Link>
          </CardHeader>
          <CardBody>
            <GridContainer>
              <GridItem xs={12} sm={6}>
                <GridContainer>
                  <GridItem xs={12} sm={4} md={4}>
                    <FormLabel className={classes.labelHorizontal}>
                      Service Type
                    </FormLabel>
                  </GridItem>
                  <GridItem xs={12} sm={8} md={6}>
                    <CustomInput
                      id="service_type"
                      formControlProps={{
                        fullWidth: true,
                      }}
                      inputProps={{
                        disabled: true,
                        value: "Health Certificate",
                      }}
                    />
                  </GridItem>
                </GridContainer>
                <GridContainer>
                  <GridItem xs={12} sm={4} md={4}>
                    <FormLabel className={classes.labelHorizontal}>
                      Requester Name
                    </FormLabel>
                  </GridItem>
                  <GridItem xs={12} sm={8} md={6}>
                    <CustomInput
                      id="requester_name"
                      formControlProps={{
                        fullWidth: true,
                      }}
                      inputProps={{
                        disabled: true,
                        value: "Organization XXX",
                      }}
                    />
                  </GridItem>
                </GridContainer>
                <GridContainer>
                  <GridItem xs={12} sm={4} md={4}>
                    <FormLabel className={classes.labelHorizontal}>
                      Request Date
                    </FormLabel>
                  </GridItem>
                  <GridItem xs={12} sm={7} md={6}>
                    <CustomInput
                      id="request_date"
                      formControlProps={{
                        fullWidth: true,
                      }}
                      inputProps={{
                        disabled: true,
                        value: "2019-18-05",
                      }}
                    />
                  </GridItem>
                </GridContainer>
              </GridItem>
              <GridItem xs={12} sm={6}>
                <GridContainer>
                  <GridItem xs={12} sm={12} md={12}>
                    <FormLabel
                      style={{
                        fontWidth: "bold",
                        fontSize: "larger",
                        textDecoration: "underline",
                      }}
                    >
                      List of Required Document Attachments
                    </FormLabel>
                  </GridItem>
                </GridContainer>
                <form onSubmit={handleSubmit(onSubmitLogin)} key={1}>
                  {documents.map((_item, index) => {
                    return (
                      <GridContainer key={_item.id}>
                        <GridItem xs={12} sm={8} md={6}>
                          <FormLabel className={classes.labelHorizontal}>
                            <Link
                              target="_blank"
                              download={_item.name}
                              to={_item.fileUrl}
                            >
                              {_item.name}
                            </Link>
                          </FormLabel>
                        </GridItem>

                        <GridItem xs={12} sm={4} md={4}>
                          <FormControl
                            fullWidth
                            className={classes.labelHorizontal}
                          >
                            <InputLabel
                              htmlFor={`doc${_item.id}`}
                              className={classes.labelHorizontal}
                            >
                              Status
                            </InputLabel>
                            <Controller
                              control={control}
                              name={`doc${_item.id}`}
                              defaultValue={""}
                              render={({ onChange, value, name }) => (
                                <Select
                                  value={value ? value : ""}
                                  name={name}
                                  id={`doc${_item.id}`}
                                  MenuProps={{
                                    className: classes.selectMenu,
                                  }}
                                  classes={{
                                    select: classes.select,
                                  }}
                                  onChange={(event) => {
                                    onChange(event);
                                    handleStatusChange(event, _item.id);
                                  }}
                                >
                                  <MenuItem
                                    disabled
                                    classes={{
                                      root: classes.selectMenuItem,
                                    }}
                                  >
                                    Choose Status
                                  </MenuItem>
                                  {statusList.map((_item, index) => {
                                    return (
                                      <MenuItem
                                        key={_item.id}
                                        classes={{
                                          root: classes.selectMenuItem,
                                          selected:
                                            classes.selectMenuItemSelected,
                                        }}
                                        value={_item.id}
                                      >
                                        {_item.name}
                                      </MenuItem>
                                    );
                                  })}
                                </Select>
                              )}
                            />
                            {_item.status === 3 ? <RejectRemark /> : null}
                          </FormControl>
                        </GridItem>
                      </GridContainer>
                    );
                  })}
                  {showRequestStatus ? (
                    <GridContainer>
                      <GridItem xs={12} sm={4} md={4}>
                        <FormLabel className={classes.labelHorizontal}>
                          Request Status
                        </FormLabel>
                      </GridItem>
                      <GridItem xs={12} sm={8} md={6}>
                        <FormControl
                          fullWidth
                          className={classes.labelHorizontal}
                        >
                          <InputLabel
                            htmlFor="requestStatus"
                            className={classes.labelHorizontal}
                          >
                            Status
                          </InputLabel>
                          <Controller
                            control={control}
                            name="requestStatus"
                            defaultValue={""}
                            as={
                              <Select
                                id="requestStatus"
                                MenuProps={{
                                  className: classes.selectMenu,
                                }}
                                classes={{
                                  select: classes.select,
                                }}
                                onChange={handleRequestStatusChange}
                              >
                                <MenuItem
                                  disabled
                                  classes={{
                                    root: classes.selectMenuItem,
                                  }}
                                >
                                  Choose Status
                                </MenuItem>
                                {requestStatusList
                                  .filter(
                                    (value) => !anyDocRejected || value.id == 1
                                  )
                                  .map((_item, index) => {
                                    return (
                                      <MenuItem
                                        key={_item.id}
                                        classes={{
                                          root: classes.selectMenuItem,
                                          selected:
                                            classes.selectMenuItemSelected,
                                        }}
                                        value={_item.id}
                                      >
                                        {_item.name}
                                      </MenuItem>
                                    );
                                  })}
                              </Select>
                            }
                          />
                        </FormControl>
                      </GridItem>
                    </GridContainer>
                  ) : null}

                  <Button color="warning" size="sm" type="submit">
                    {/* <Link
                      style={{ color: "white" }}
                      to="/tradePromotion/trade_facilitation"
                    >
                      {"Save"}
                    </Link> */}
                    {"Save"}
                  </Button>
                  <Button color="primary" size="sm">
                    <Link
                      style={{ color: "white" }}
                      to="/tradePromotion/trade_facilitation"
                    >
                      {"Cancel"}
                    </Link>
                  </Button>
                </form>
              </GridItem>
            </GridContainer>
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}
