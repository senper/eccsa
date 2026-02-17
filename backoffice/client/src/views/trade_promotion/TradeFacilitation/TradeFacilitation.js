import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Button from "components/vender/CustomButtons/Button.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";
import ReactTable from "components/vender/ReactTable/ReactTable.js";

import { serviceRequestsData } from "variables/service_requests_data.js";
import { serviceTypes } from "variables/lookups.js";

import { cardTitle } from "assets/jss/material-dashboard-pro-react.js";
import Info from "@material-ui/icons/Info";
import { Link } from "react-router-dom";
import { Chip } from "@material-ui/core";

import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";

const styles = {
  cardIconTitle: {
    ...cardTitle,
    marginTop: "15px",
    marginBottom: "0px",
  },
};

const useStyles = makeStyles(styles);

export default function MessageTemplates() {
  const [data, setData] = React.useState(
    serviceRequestsData.map((prop, key) => {
      return {
        id: key,
        service_type: prop.service_type,
        requester_name: prop.requester_name,
        request_date: prop.request_date,
        payment_receipt_no: prop.payment_receipt_no,
        status: <Chip label={"Active"} color="primary" />,
        actions: (
          // we've added some custom button actions
          <div className="actions-right">
            {/* use this button to add a like kind of action */}

            <Link
              color="info"
              to="/tradePromotion/trade_facilitation_detail"
              className="btn btn-primary"
            >
              <Info />
            </Link>
          </div>
        ),
      };
    })
  );
  const classes = useStyles();

  const [serviceType, setServiceType] = React.useState("");
  const handleServiceType = (event) => {
    setServiceType(event.target.value);
  };
  return (
    <GridContainer>
      <GridItem xs={12}>
        <Card>
          <CardHeader color="primary" icon>
            <CardIcon color="primary">{"Current Active Services"}</CardIcon>
          </CardHeader>
          <CardBody>
            <GridContainer>
              <GridItem xs={12} sm={12} md={4}>
                <FormControl fullWidth className={classes.selectFormControl}>
                  <InputLabel
                    htmlFor="service-type"
                    className={classes.selectLabel}
                  >
                    Choose Service Type
                  </InputLabel>
                  <Select
                    MenuProps={{
                      className: classes.selectMenu,
                    }}
                    classes={{
                      select: classes.select,
                    }}
                    value={serviceType}
                    onChange={handleServiceType}
                    inputProps={{
                      name: "ServiceType",
                      id: "service-type",
                    }}
                  >
                    <MenuItem
                      disabled
                      classes={{
                        root: classes.selectMenuItem,
                      }}
                    >
                      Choose Service Type
                    </MenuItem>
                    {serviceTypes.map((_item, index) => {
                      return (
                        <MenuItem
                          key={_item.id}
                          classes={{
                            root: classes.selectMenuItem,
                            selected: classes.selectMenuItemSelected,
                          }}
                          value={_item.id}
                        >
                          {_item.name}
                        </MenuItem>
                      );
                    })}
                  </Select>
                </FormControl>
              </GridItem>
            </GridContainer>
            <br />
            <ReactTable
              columns={[
                {
                  Header: "Requests",
                  columns: [
                    {
                      Header: "Service Type",
                      accessor: "service_type",
                      width: 500,
                    },
                    {
                      Header: "Requester Name",
                      accessor: "requester_name",
                    },
                    {
                      Header: "Request Date",
                      accessor: "request_date",
                    },
                    {
                      Header: "Payment Receipt No",
                      accessor: "payment_receipt_no",
                    },
                    {
                      Header: "Status",
                      accessor: "status",
                    },
                    {
                      Header: "Actions",
                      accessor: "actions",
                      width: 10,
                      minWidth: 10,
                      maxWidth: 10,
                    },
                  ],
                },
              ]}
              data={data}
            />
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}
