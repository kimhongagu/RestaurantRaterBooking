<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Model;

class VanPhongPham extends Model
{
    protected $table = 'vanphongpham';

    protected $fillable = [
        'danhmuc_id',
        'hangsanxuat_id',
        'tenvpp',
        'mota',
        'giavpp',
        'giasale',
        'trangthai',
        'soluong',
        'hinhanh',
    ];

    public function chiTietDonHang()
    {
        return $this->hasMany(ChiTietDonHang::class, 'vpp_id', 'id');
    }
}
