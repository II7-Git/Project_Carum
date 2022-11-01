package com.a101.carum.api.controller;

import com.a101.carum.api.dto.*;
import com.a101.carum.service.JwtService;
import com.a101.carum.service.ShopService;
import com.a101.carum.service.TestService;
import lombok.RequiredArgsConstructor;
import lombok.extern.log4j.Log4j2;
import org.springframework.data.domain.Pageable;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;

@RestController
@RequiredArgsConstructor
@RequestMapping("shop")
public class ShopController {
   private final ShopService shopService;
   private final JwtService jwtService;
   @GetMapping()
   public ResponseEntity readFurnitureList(@ModelAttribute ReqGetFurnitureList reqGetFurnitureList, Pageable pageable, HttpServletRequest request){
      Long id = jwtService.getUserId(request);
      return ResponseEntity.ok(shopService.readFurnitureList(reqGetFurnitureList, pageable, id));
   }

   @PostMapping()
   public ResponseEntity postInventory(@RequestBody ReqPostInventory reqPostInventory, HttpServletRequest request){
      Long id = jwtService.getUserId(request);
      shopService.postInventory(reqPostInventory, id);
      return ResponseEntity.ok().build();
   }
}
